using System.Diagnostics;

namespace ShellRunner;

public abstract class ProcessCommand 
{

    string _command;
    public ProcessCommand(string command)
    {
        _command = command;
    }

    public string Command 
        => _command;

    public void RunCommand(Process process)
    {
        try
        {
            Run(process);
        }
        catch(Exception ex)
        {
            var fg = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error while running command.");
            Console.WriteLine(ex);
            Console.ForegroundColor = fg;
        }
    }

    protected abstract void Run(Process process);
}
