using System;
using System.Diagnostics;
using System.Text;

namespace CommandBuilder;

public class CommandBuilder
{
    List<string> commands;
    public CommandBuilder()
    {
        commands = new List<string>();
    }
    public CommandBuilder AddCommand(string cmd)
    {
        commands.Add(cmd);

        return this;
    }

    public async Task RunAsync()
    {
        var proc = new Process();
        proc.StartInfo.FileName = "cmd";
        proc.StartInfo.Arguments = "/k";
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        proc.StartInfo.UseShellExecute = false;
        
        proc.StartInfo.RedirectStandardInput = true;
        proc.StartInfo.RedirectStandardError = true;
        proc.StartInfo.RedirectStandardOutput = true;
        
        proc.OutputDataReceived += Proc_OutputDataReceived;
        proc.ErrorDataReceived += Proc_ErrorDataReceived;

        proc.Start();

        foreach(var cmd in commands)
        {
            proc.StandardInput.WriteLine(cmd);
            proc.StandardInput.Flush();

            Console.WriteLine(cmd);
        }
        
        proc.StandardInput.WriteLine("exit");

        proc.BeginOutputReadLine();
        proc.BeginOutputReadLine();

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
