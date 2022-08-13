using System.Diagnostics;

namespace ProcessRunner;

public class GetOutputCommand : ProcessCommand
{
    Process? _process;

    public GetOutputCommand(string command)
        : base(command) { }

    public string? Output { get; private set; }

    protected override void Run(Process process)
    {
        _process = process;
        _process.OutputDataReceived += Process_OutputDataReceived;

        process.StandardInput.WriteLine(Command);
    }

    bool _outputIncoming = false;
    private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        try
        { 
            if(e.Data is not null)
            {
                if(_outputIncoming)
                {
                    Output = e.Data;
                    _outputIncoming = false;
                    _process!.OutputDataReceived -= Process_OutputDataReceived;
                }

                if (e.Data.EndsWith(Command))
                {
                    _outputIncoming = true;
                }
            }
        }
        catch(Exception ex)
        {
            var fg = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex);
            Console.ForegroundColor = fg;
        }
    }
}
