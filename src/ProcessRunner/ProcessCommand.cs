using System.Diagnostics;

namespace ProcessRunner;

public abstract class ProcessCommand 
{
    public abstract void Run(Process process);
}
