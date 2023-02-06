using CommandLine;
using DriveSync;

await Parser.Default.ParseArguments<Options>(args)
    .WithParsedAsync(Run);

void Run(Options options)
{
    try
    {
        Config config = new(options.Config);
        SyncManager manager = new(config.Items);
    }
    catch (Config.InvalidConfigException e)
    {
        Console.WriteLine(e.Message);
    }
}