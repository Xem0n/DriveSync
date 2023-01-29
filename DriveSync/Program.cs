using CommandLine;
using DriveSync;

CommandLine.Parser.Default.ParseArguments<Options>(args)
    .WithParsed(Run);

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