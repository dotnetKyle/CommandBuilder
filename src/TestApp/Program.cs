using CommandBuilder;
using System.Reflection;


var execAssemblyLocation = Assembly.GetExecutingAssembly().Location;
var directory = Path.GetDirectoryName(execAssemblyLocation);
if (directory is null)
    throw new Exception();

new CommandBuilder.CommandBuilder()
    .UsePowershell()
    .AddWorkingDirectory(directory)
    .AddCommand("cd ../../../../MyFakeLibrary")
    .AddCommand("dotnet --info")
    .AddCommand("dotnet build MyFakeLibrary.csproj -c Release")
    .AddCommand("cd bin/Release/netstandard2.0")
    .AddCommand("Get-Childitem")
    .AddCommand("Add-Type -Path .\\MyFakeLibrary.dll")
    .AddCommand("$obj = new-object MyFakeLibrary.TestClass")
    .AddCommand("$obj.TestLibrary(\"test test\")")
    .Run();