namespace DriveSync;

public class SyncManager
{
    private DriveServiceFacade _service = new();
    private List<Item> _items;

    public SyncManager(List<Item> items)
    {
        _items = items;
    }
}