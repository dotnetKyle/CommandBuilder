using System.Diagnostics;

namespace ProcessRunner;

public class TypicalCommand : ProcessCommand
{
    string _command;
    public TypicalCommand(string command)
    {
        _command = command;
    }

    protected override void Run(Process process)
    {
        process.StandardInput.WriteLine(_command);
    }
}
