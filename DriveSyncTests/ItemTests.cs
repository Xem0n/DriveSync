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
        item.LocalPath = "invalid path";
        
        Assert.That(item.IsValid(), Is.False);
    }

    [Test]
    public void IsValid_NotExistingLocalPath()
    {
        Item item = CreateValidItem(false);
        item.LocalPath = "/tmp/not/existing/path/example.file";
        
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
        cache = item.LocalPath;
        item.LocalPath = null;
        Assert.That(item.IsValid(), Is.False);

        item.LocalPath = cache;
        item.DrivePath = null;
        Assert.That(item.IsValid(), Is.False);

        item.Rule = null;
        item.LocalPath = null;
        Assert.That(item.IsValid(), Is.False);
    }

    private Item CreateValidItem(bool shouldCreateLocalFile = true)
    { 
        const string path = "/tmp/example.file";

        if (shouldCreateLocalFile)
        {
            using (FileStream fs = File.Create(path))
            {
                fs.Dispose();
            }
        }
        
        return new()
        { 
            LocalPath = path,
            DrivePath = "remote/path/to/example.file",
            Rule = "sync"
        };
    }
}