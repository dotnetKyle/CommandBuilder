using System.Diagnostics;

namespace ProcessRunner;

public class GetOutputCommand : ProcessCommand
{
    string _command;
    Process? _process;

    public GetOutputCommand(string command)
    {
        _command = command;
    }

    public string Command => _command;
    public string? Output { get; private set; }

    protected override void Run(Process process)
    {
        _process = process;
        _process.OutputDataReceived += Process_OutputDataReceived;

        process.StandardInput.WriteLine(_command);
    }

    bool _outputIncoming = false;
    private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        try
        { 
            if(_outputIncoming)
            {
                Output = e.Data;
                _outputIncoming = false;
                _process!.OutputDataReceived -= Process_OutputDataReceived;
            }

            if (e.Data is not null && e.Data.EndsWith(_command))
            {
                _outputIncoming = true;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
