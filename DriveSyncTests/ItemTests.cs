namespace DriveSyncTests;

public class ItemTests
{
    [Test]
    public void ShouldBeValid()
    {
        const string path = "/tmp/example.file";
        
        File.Create(path);

        Item item = new()
        {
            Local = path,
            Drive = "remote/path/to/example.file",
            Rule = "sync"
        };
        
        Assert.That(item.IsValid(), Is.True);
    }
}