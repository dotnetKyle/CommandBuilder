using System;
using System.Diagnostics;

namespace ProcessRunner;
public class CommandBuilder
{
    CommandBuilderOptions _options;

    public ProcessStartInfo StartInfo { get; set; }
    public Process Process { get; set; }

    internal CommandBuilder(CommandBuilderOptions options, Process process)
    {
        Process = process;

        Commands = new List<ProcessCommand>();
        
        _options = options;

        StartInfo = new ProcessStartInfo
        {
            FileName = options.File,
            Arguments = options.Args,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            UseShellExecute = false,
            RedirectStandardOutput = options.RedirectStandardOutput,
            RedirectStandardError = options.RedirectStandardError, 
            RedirectStandardInput = options.RedirectStandardInput
        };
    }

    public List<ProcessCommand> Commands { get; private set; }

    public CommandBuilder AddWorkingDirectory(string workingDirectory)
    {
        StartInfo.WorkingDirectory = workingDirectory;
        return this;
    }

    private void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        try
        {
            if (e.Data is null)
                return;

            var fg = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Error: {e.Data}");
            Console.ForegroundColor = fg;
        }
        catch(Exception ex)
        {
            var fg = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error while processing error data.");
            Console.WriteLine(ex);
            Console.ForegroundColor = fg;
        }
    }

    private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        try
        {
            Console.WriteLine(e.Data);
        }
        catch (Exception ex)
        {
            var fg = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error while processing output data.");
            Console.WriteLine(ex);
            Console.ForegroundColor = fg;
        }
    }
}
