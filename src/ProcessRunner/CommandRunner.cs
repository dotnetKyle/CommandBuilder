using System.Diagnostics;

namespace ProcessRunner;

public static class CommandRunner
{
    public static CommandBuilderOptions UsePowershell()
    {
        var options = new CommandBuilderOptions(
            ShellType.Powershell, 
            "powershell",
            "")
        {
            RedirectStandardInput = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true, 
        };
        return options;
    }
    public static CommandBuilderOptions UseBash()
    {
        var options = new CommandBuilderOptions(
            ShellType.Bash,
            "/bin/bash",
            "-v")
        {
            RedirectStandardInput = true, 
            RedirectStandardError = true, 
            RedirectStandardOutput = true
        };
        return options;
    }
    public static CommandBuilderOptions UseWindowsCommandShell()
    {
        var options = new CommandBuilderOptions(
            ShellType.WindowsCommandShell,
            "cmd",
            "/k")
        {
            RedirectStandardInput = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true
        };
        return options;
    }

    public static CommandBuilderOptions AddWorkingDirectory(
        this CommandBuilderOptions options, 
        string workingDirectory)
    {
        options = options with 
        { 
            WorkingDirectory = workingDirectory 
        };

        return options;
    }

    public static CommandBuilder StartProcess(this CommandBuilderOptions options)
    {
        var proc = new Process();

        proc.StartInfo.FileName = options.File;
        proc.StartInfo.Arguments = options.Args;
        proc.StartInfo.RedirectStandardOutput = options.RedirectStandardOutput;
        proc.StartInfo.RedirectStandardError = options.RedirectStandardError;
        proc.StartInfo.RedirectStandardInput = options.RedirectStandardInput;
        
        if(options.WorkingDirectory is not null)
            proc.StartInfo.WorkingDirectory = options.WorkingDirectory;

        proc.Start();

        proc.OutputDataReceived += (o,e) => {
            try
            {
                if(e.Data is not null)
                    PrintOutputData(e.Data);
            }
            catch (Exception ex)
            {
                PrintErrorData("Error while processing error data.");
                PrintErrorData(ex.ToString());
            }
        };
        proc.ErrorDataReceived += (o,e) => {
            try
            {
                if (e.Data is not null)
                    PrintErrorData(e.Data);
            }
            catch (Exception ex)
            {
                PrintErrorData("Error while processing error data.");
                PrintErrorData(ex.ToString());
            }
        };

        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();

        var builder = new CommandBuilder(options, proc);

        return builder;
    }

    public static CommandBuilder AddCommand(this CommandBuilder builder, string command)
    {
        builder.Commands.Add(new TypicalCommand(command));
        return builder;
    }
    public static CommandBuilder AddCommandWithOutput(this CommandBuilder builder, string command)
    {
        builder.Commands.Add(new GetOutputCommand(command));
        return builder;
    }

    public static List<CommandOutput> Run(this CommandBuilder builder)
    {
        foreach(var cmd in builder.Commands)
        {
            cmd.RunCommand(builder.Process);
            builder.Process.Refresh();
        }

        builder.Process.Refresh();

        builder.Process.StandardInput.WriteLine("exit");

        builder.Process.WaitForExit();

        builder.Process.Close();

        var outputs = new List<CommandOutput>();

        foreach(var cmd in builder.Commands)
        {
            if(cmd is GetOutputCommand)
            {
                var output = (GetOutputCommand)cmd;
                outputs.Add(new CommandOutput(output.Command, output.Output));
            }
        }

        return outputs;
    }

    private static void PrintErrorData(string data)
    {
        var fg = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(data);
        Console.ForegroundColor = fg;
    }
    private static void PrintOutputData(string data)
    {
        Console.WriteLine(data);
    }
}