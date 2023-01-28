using System.Text;
using System.Text.Json;

namespace DriveSync;

public class Config
{
    private const string DEFAULT_PATH = "/.config/drivesync/config.json";
    private const string TEMPLATE = @"
[
    {
        ""Local"": ""path to file on your disk"",
        ""Drive"": ""path to the file on your google drive"",
        ""Rule"": ""download/upload/sync""
    }
]
    ";

    private string _path;

    public List<Item> Items { get; set; }

    public Config(string path)
    {
        _path = path ?? $"/home/{Environment.GetEnvironmentVariable("USER")}{DEFAULT_PATH}";

        if (!File.Exists(_path))
        {
            CreateTemplateFile();
            
            throw NewTemplateException();
        }

        Items = Read();
        
        Verify();
    }

    void CreateTemplateFile()
    {
        string directory = Path.GetDirectoryName(_path);
        
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (FileStream fs = File.Create(_path))
        {
            byte[] template = new UTF8Encoding(true).GetBytes(TEMPLATE);
            
            fs.Write(template, 0, template.Length);
        }
    }

    List<Item> Read()
    {
        string json = File.ReadAllText(_path);
        List<Item> items = JsonSerializer.Deserialize<List<Item>>(json);

        return items;
    }

    void Verify()
    {
        if (Items.Any(item => !item.IsValid()))
        {
            throw new InvalidConfigException();
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