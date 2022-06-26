using CommandBuilder;

Console.WriteLine("running...");

await new CommandBuilder.CommandBuilder()
    .AddCommand("echo @")
    .RunAsync();

Console.WriteLine("...done");
