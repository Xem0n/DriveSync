using CommandLine;

namespace DriveSync;

public class Options
{
    [Option('c', "config", Required = false, HelpText = "Set path to config.\nDefault path: ~/.config/drivesync/config.json.\nAccepts only absolute paths.")]
    public string Config { get; set; }
}