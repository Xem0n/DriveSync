using CommandLine;
using DriveSync;

await Parser.Default.ParseArguments<Options>(args)
    .WithParsedAsync(Run);

async Task Run(Options options)
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
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}