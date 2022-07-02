using ProcessRunner;
using System.Reflection;


var execAssemblyLocation = Assembly.GetExecutingAssembly().Location;
var directory = Path.GetDirectoryName(execAssemblyLocation);
if (directory is null)
    throw new Exception();

new CommandBuilder()
    .UsePowershell()
    .AddWorkingDirectory(directory)
    // show what version of dotnet is loaded
    .AddCommand("dotnet --info")
    // cd into the lbirary directory
    .AddCommand("cd ../../../../MyFakeLibrary")
    // build 
    .AddCommand("dotnet build MyFakeLibrary.csproj -c Release")
    .AddCommand("cd bin/Release/netstandard2.0")
    // print artifacts
    .AddCommand("Get-Childitem")
    // load built project into process and run one of the methods
    .AddCommand("Add-Type -Path .\\MyFakeLibrary.dll")
    .AddCommand("$obj = new-object MyFakeLibrary.TestClass")
    .AddCommand("$obj.TestLibrary('test test')")
    .Run();