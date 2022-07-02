using System.Diagnostics;

namespace ProcessRunner;

public class CommandBuilder
{
    List<ProcessCommand> commands;
    ProcessStartInfo startInfo;

    public CommandBuilder()
    {
        commands = new List<ProcessCommand>();
        startInfo = new ProcessStartInfo
        {
            FileName = "cmd",
            Arguments = "/k",
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true, 
            RedirectStandardInput = true
        };
    }

    public CommandBuilder UsePowershell()
    {
        startInfo.FileName = "powershell";
        startInfo.Arguments = "";
        return this;
    }

    public CommandBuilder AddWorkingDirectory(string workingDirectory)
    {
        startInfo.WorkingDirectory = workingDirectory;
        return this;
    }

    public CommandBuilder AddCommand(string cmd)
    {
        var command = new TypicalCommand(cmd);
        commands.Add(command);
        return this;
    }

    public CommandBuilder AddCommandWithOutput(string cmd)
    {
        var command = new GetOutputCommand(cmd);
        commands.Add(command);
        return this;
    }

    public List<(string Command, string? Output)> Run()
    {
        var proc = new Process();

        proc.StartInfo = startInfo;

        var wd = proc.StartInfo.WorkingDirectory;

        proc.OutputDataReceived += Proc_OutputDataReceived;
        proc.ErrorDataReceived += Proc_ErrorDataReceived;

        proc.Start();

        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();

        foreach(var cmd in commands)
        {
            cmd.Run(proc);

            proc.Refresh();
        }

        proc.Refresh();

        proc.StandardInput.WriteLine("exit");
        proc.WaitForExit();

        proc.Close();

        var _output = new List<(string Command, string? Output)>();

        foreach (var cmd in commands)
        { 
            if(cmd is GetOutputCommand)
            {
                var outputCmd = (GetOutputCommand)cmd;
                _output.Add(new(outputCmd.Command, outputCmd.Output));
            }
        }

        return _output;
    }

    private void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data is null)
            return;

        var fg = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: " + e.Data);
        Console.ForegroundColor = fg;
    }

    private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        Console.WriteLine(e.Data);
    }
}
