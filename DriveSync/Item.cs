namespace DriveSync;

public class Item
{
    public string Local { get; set; }
    public string Drive { get; set; }
    public string Rule { get; set; }

    public bool IsValid()
    {
        return true;
    }
}