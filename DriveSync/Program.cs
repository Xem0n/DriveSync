using CommandLine;
using DriveSync;

CommandLine.Parser.Default.ParseArguments<Options>(args)
    .WithParsed(Run);

void Run(Options options)
{
    Config config = new(options.Config);
}