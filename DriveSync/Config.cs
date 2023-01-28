namespace DriveSync;

public class Config
{
    private const string DEFAULT_PATH = "~/.config/drivesync/config.json";

    public Config(string path)
    {
        path ??= DEFAULT_PATH;

        if (!File.Exists(path))
        {
            // todo: create template
        }
        
        // todo: create dict of json (or sth like that, im not sure yet)
    }
}