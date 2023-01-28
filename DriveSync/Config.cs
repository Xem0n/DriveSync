using System.Text;

namespace DriveSync;

public class Config
{
    private const string DEFAULT_PATH = "/.config/drivesync/config.json";
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
        path ??= $"/home/{Environment.GetEnvironmentVariable("USER")}{DEFAULT_PATH}";

        if (!File.Exists(path))
        {
            CreateTemplate(path);
            
            throw NewTemplateException();
        }
        
        // todo: create dict of json (or sth like that, im not sure yet)
    }

    void CreateTemplate(string path)
    {
        string directory = Path.GetDirectoryName(path);
        
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (FileStream fs = File.Create(path))
        {
            byte[] template = new UTF8Encoding(true).GetBytes(TEMPLATE);
            
            fs.Write(template, 0, template.Length);
        }
    }

    InvalidConfigException NewTemplateException()
    {
        const string message = "Template has been created. Fill it and try again.";

        return new(message);
    }

    public class InvalidConfigException : Exception
    {
        public InvalidConfigException() : base("Config found in your config file is invalid.") {}
        
        public InvalidConfigException(string message) : base(message) {}
    }
}