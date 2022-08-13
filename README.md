# ProcessRunner

Run command line tools inside CSharp.

Example running some dotnet CLI commands:

```csharp
CommandRunner()
    .UsePowershell() 
    .StartProcess()
    // print the dotnet info command
    .AddCommand("dotnet --info")
    // build a csharp project
    .AddCommand("cd C:\\source\\repos\\MyProject")
    .AddCommand("dotnet build MyProject.sln -c Release")
    // Print the output
    .AddCommand("cd bin\\Release")
    .AddCommand("Get-ChildItem -r")
    .Run();
```

Process Runner is really useful to create a wrapper for several other 
commands and to work with data before or after you run the commands.