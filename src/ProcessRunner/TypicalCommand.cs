using System.Diagnostics;

namespace ProcessRunner;

public class TypicalCommand : ProcessCommand
{
    string _command;
    public TypicalCommand(string command)
    {
        _command = command;
    }

    public override void Run(Process process)
    {
        process.StandardInput.WriteLine(_command);
    }
}
