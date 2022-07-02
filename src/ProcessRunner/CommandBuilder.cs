using System.Diagnostics;

namespace ProcessRunner;

public class CommandBuilder
{
    List<string> commands;
    ProcessStartInfo startInfo;

    public CommandBuilder()
    {
        commands = new List<string>();
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
        commands.Add(cmd);

        return this;
    }

    public void Run()
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
            proc.StandardInput.WriteLine(cmd);

            proc.Refresh();
        }

        proc.OutputDataReceived -= Proc_OutputDataReceived;
        proc.ErrorDataReceived -= Proc_ErrorDataReceived;

        proc.StandardInput.WriteLine("exit");
        proc.WaitForExit();

        proc.Close();
    }

    private void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        var fg = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.Data);
        Console.ForegroundColor = fg;
    }
    private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        Console.WriteLine(e.Data);
    }
}
