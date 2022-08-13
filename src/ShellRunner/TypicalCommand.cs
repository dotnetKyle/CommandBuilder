using System.Diagnostics;

namespace ShellRunner;

public class TypicalCommand : ProcessCommand
{
    public TypicalCommand(string command)
        : base(command) { }

    protected override void Run(Process process)
    {
        process.StandardInput.WriteLine(Command);
    }
}
