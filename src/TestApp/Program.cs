using ShellRunner;
using System.Reflection;

try
{
    var fg = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("v10");
    Console.ForegroundColor = fg;

    var execAssemblyLocation = Assembly.GetExecutingAssembly().Location;
    var directory = Path.GetDirectoryName(execAssemblyLocation);

    if (directory is null)
        throw new Exception("Directory null exception");


    if(args.Length == 0)
    {
        Console.WriteLine("Provide an argument: 'bash', 'powershell', or 'cmd'");
        return;
    }

    List<CommandOutput>? firstOutput = null;
    if (args[0] == "bash")
    {
        firstOutput = CommandRunner
            .UseBash()
            .StartProcess()
            .AddCommand("dotnet --info")
            .AddCommand("echo test")
            .AddCommandWithOutput("echo foo")
            .AddCommandWithOutput("echo bar")
            .Run();
    }
    else if (args[0] == "powershell")
    {
        firstOutput = CommandRunner
            .UsePowershell()
            .StartProcess()
            .AddCommand("dotnet --info")
            .AddCommand("echo PowerShell")
            .AddCommandWithOutput("echo foo")
            .AddCommandWithOutput("echo bar")
            .Run();
    }
    else if (args[0] == "cmd")
    {
        firstOutput = CommandRunner
            .UseWindowsCommandShell()
            .StartProcess()
            .AddCommand("echo off")
            .AddCommand("dotnet --info")
            .AddCommand("echo CMD")
            .AddCommandWithOutput("echo foo")
            .AddCommandWithOutput("echo bar")
            .Run();
    }
    else
    {
        Console.WriteLine("Please choose bash powershell or cmd.");
        return;
    }

    if (firstOutput is null)
        return;

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Outputs:");
    foreach (var o in firstOutput)
        Console.WriteLine("  " + o.Output);

    Console.ForegroundColor = ConsoleColor.Gray;

    if(args[0] == "powershell")
    {
        var secondOutput = CommandRunner
            .UsePowershell() 
            .StartProcess()
            .AddCommand("dotnet --info")
            .AddWorkingDirectory(directory)
            .AddCommand("$myVar = 'foo'")
            .AddCommand("echo $myVar")
            // show what version of dotnet is loaded
            .AddCommand("dotnet --info")
            // cd into the lbirary directory
            .AddCommand("cd ../../../../MyFakeLibrary")
            // build 
            .AddCommand("dotnet build MyFakeLibrary.csproj -c Release")
            .AddCommand("cd bin/Release/netstandard2.0")
            //.AddCommand("echo $myVar")
            // print artifacts
            .AddCommandWithOutput("Get-Childitem")
            // load built project into process and run one of the methods
            .AddCommand("Add-Type -Path .\\MyFakeLibrary.dll")
            .AddCommand("$obj = new-object MyFakeLibrary.TestClass")
            .AddCommand("$obj.TestLibrary('test test')")
            .Run();


        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Outputs (2):");
        foreach (var o in secondOutput)
            Console.WriteLine("  " + o.Output);

        Console.ForegroundColor = ConsoleColor.Gray;
    }

}
catch (Exception ex)
{
    Console.WriteLine("Unhandled error in TestApp.");
    Console.WriteLine(ex);
}