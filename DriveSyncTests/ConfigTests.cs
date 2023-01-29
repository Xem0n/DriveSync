using System.Text;
using System.Text.Json;

namespace DriveSyncTests;

public class ConfigTests
{
    private const string DEFAULT_PATH = "/.config/drivesync/config.json";
    private const string CUSTOM_PATH = "/tmp/example.config.json";
    
    private const string INVALID_CONFIG_EXCEPTION = "Config found in your config file is invalid.";
    private const string NEW_TEMPLATE_EXCEPTION = "Template has been created. Fill it and try again.";

    private const string INVALID_CONFIG = "{[[}]]]}";
    private const string INVALID_ITEM_CONFIG = @"
[
    {
        ""Local"": ""invalid path"",
        ""Drive"": ""invalid path"",
        ""Rule"": ""invalid rule""
    }
]
    ";
    private const string VALID_CONFIG = @"
[
    {
        ""Local"": ""/tmp/example.file"",
        ""Drive"": ""remote/path/to/example.file"",
        ""Rule"": ""sync""
    }
]
    ";

    [Test]
    public void DefaultPath_NotExisting_ShouldThrowException()
    {
        File.Delete(GetDefaultPath());

        try
        {
            Config config = new(null);
        }
        catch (Config.InvalidConfigException error)
        {
            StringAssert.Contains(error.Message, NEW_TEMPLATE_EXCEPTION);
        }
    }

    [Test]
    public void CustomPath_NotExisting_ShouldThrowException()
    {
        File.Delete(CUSTOM_PATH);
        
        try
        {
            Config config = new(CUSTOM_PATH);
        }
        catch (Config.InvalidConfigException error)
        {
            StringAssert.Contains(error.Message, NEW_TEMPLATE_EXCEPTION);
        }
    }

    [Test]
    public void DefaultPath_WithValidConfig()
    {
        CreateConfigFile(GetDefaultPath(), VALID_CONFIG);
        CreateExampleFile();

        Assert.DoesNotThrow(() => new Config(null));
    }

    [Test]
    public void DefaultPath_WithInvalidConfig_ShouldThrowException()
    {
        CreateConfigFile(GetDefaultPath(), INVALID_CONFIG);

        Assert.Throws<JsonException>(() => new Config(null));
    }

    [Test]
    public void CustomPath_WithValidConfig()
    {
        CreateConfigFile(CUSTOM_PATH, VALID_CONFIG);
        CreateExampleFile();

        Assert.DoesNotThrow(() => new Config(CUSTOM_PATH));   
    }

    [Test]
    public void CustomPath_WithInvalidConfig_ShouldThrowException()
    {
        CreateConfigFile(CUSTOM_PATH, INVALID_CONFIG);

        Assert.Throws<JsonException>(() => new Config(CUSTOM_PATH));
    }

    [Test]
    public void DefaultPath_WithInvalidItem_ShouldThrowException()
    {
        CreateConfigFile(GetDefaultPath(), INVALID_ITEM_CONFIG);

        try
        {
            Config config = new(null);
        }
        catch (Config.InvalidConfigException error)
        {
            StringAssert.Contains(error.Message, INVALID_CONFIG_EXCEPTION);
        }
    }
    
    [Test]
    public void CustomPath_WithInvalidItem_ShouldThrowException()
    {
        CreateConfigFile(CUSTOM_PATH, INVALID_ITEM_CONFIG);

        try
        {
            Config config = new(CUSTOM_PATH);
        }
        catch (Config.InvalidConfigException error)
        {
            StringAssert.Contains(error.Message, INVALID_CONFIG_EXCEPTION);
        }
    }

    private void CreateConfigFile(string path, string json)
    {
        File.Delete(path);
        
        using (FileStream fs = File.Create(path))
        {
            byte[] config = new UTF8Encoding(true).GetBytes(json);
            
            fs.Write(config, 0, json.Length);
        }       
    }

    private void CreateExampleFile()
    {
        using (FileStream fs = File.Create("/tmp/example.file"))
        {
            fs.Dispose();
        }
    }

    private string GetDefaultPath() => $"/home/{Environment.GetEnvironmentVariable("USER")}{DEFAULT_PATH}";
}