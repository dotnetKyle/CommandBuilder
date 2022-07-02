using ProcessRunner;
using System.Reflection;

try
{
    var execAssemblyLocation = Assembly.GetExecutingAssembly().Location;
    var directory = Path.GetDirectoryName(execAssemblyLocation);

    if (directory is null)
        throw new Exception("Directory null exception");

    var output = new CommandBuilder()
        .UseBash()
        .AddCommand("dotnet --info")
        .AddCommand("echo test")
        .AddCommandWithOutput("echo foo")
        //.UsePowershell()
        //.AddWorkingDirectory(directory)
        //.AddCommand("$myVar = 'foo'")
        //.AddCommandWithOutput("echo $myVar")
        //// show what version of dotnet is loaded
        //.AddCommand("dotnet --info")
        //// cd into the lbirary directory
        //.AddCommand("cd ../../../../MyFakeLibrary")
        //// build 
        //.AddCommand("dotnet build MyFakeLibrary.csproj -c Release")
        //.AddCommand("cd bin/Release/netstandard2.0")
        ////.AddCommand("echo $myVar")
        //// print artifacts
        ////.AddCommand("Get-Childitem")
        //.AddCommandWithOutput("echo bar")
        //// load built project into process and run one of the methods
        //.AddCommand("Add-Type -Path .\\MyFakeLibrary.dll")
        //.AddCommand("$obj = new-object MyFakeLibrary.TestClass")
        //.AddCommand("$obj.TestLibrary('test test')")
        .Run();


    Console.WriteLine("Outputs:");
    Console.ForegroundColor = ConsoleColor.Blue;

    foreach (var o in output)
        Console.WriteLine("  " + o.Output);

    Console.ForegroundColor = ConsoleColor.Gray;
}
catch(Exception ex)
{
    Console.WriteLine("Unhandled error in TestApp.");
    Console.WriteLine(ex);
}