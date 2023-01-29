namespace DriveSyncTests;

public class ItemTests
{
    [Test]
    public void IsValid_ShouldBeValid()
    {
        Item item = CreateValidItem();
        Assert.That(item.IsValid(), Is.True);

        item.Rule = "download";
        Assert.That(item.IsValid(), Is.True);
        
        item.Rule = "upload";
        Assert.That(item.IsValid(), Is.True);
    }

    [Test]
    public void IsValid_InvalidRule()
    {
        Item item = CreateValidItem();
        item.Rule = "invalid";
        
        Assert.That(item.IsValid(), Is.False);
    }

    [Test]
    public void IsValid_InvalidLocalPath()
    {
        Item item = CreateValidItem(false);
        item.Local = "invalid path";
        
        Assert.That(item.IsValid(), Is.False);
    }

    [Test]
    public void IsValid_InvalidNullAnyProperty()
    {
        Item item = CreateValidItem();

        string cache = item.Rule;
        item.Rule = null;
        Assert.That(item.IsValid(), Is.False);

        item.Rule = cache;
        cache = item.Local;
        item.Local = null;
        Assert.That(item.IsValid(), Is.False);

        item.Local = cache;
        item.Drive = null;
        Assert.That(item.IsValid(), Is.False);

        item.Rule = null;
        item.Local = null;
        Assert.That(item.IsValid(), Is.False);
    }

    private Item CreateValidItem(bool shouldCreateLocalFile = true)
    { 
        const string path = "/tmp/example.file";

        if (shouldCreateLocalFile)
        {
            File.Create(path);
        }
        
        return new()
        { 
            Local = path,
            Drive = "remote/path/to/example.file",
            Rule = "sync"
        };
    }
}