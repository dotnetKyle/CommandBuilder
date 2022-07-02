using System.Diagnostics;

namespace ProcessRunner;

public abstract class ProcessCommand 
{
    public void RunCommand(Process process)
    {
        try
        {
            Run(process);
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error while running command.");
            Console.WriteLine(ex);
        }
    }

    protected abstract void Run(Process process);
}
