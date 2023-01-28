namespace DriveSync;

public class Config
{
    private const string DEFAULT_PATH = "~/.config/drivesync/config.json";
    private const string TEMPLATE = @"
{
    ""files\"": [
        ""local"": ""path to file on your disk"",
        ""drive"": ""path to the file on your google drive"",
        ""rule"": ""download/upload/sync""
    ]
}
    ";

    public Config(string path)
    {
        Console.WriteLine(TEMPLATE);
        path ??= DEFAULT_PATH;

        if (!File.Exists(path))
        {
            // todo: create template
        }
        
        // todo: create dict of json (or sth like that, im not sure yet)
    }
}