namespace DriveSync;

public class SyncManager
{
    private DriveServiceFacade _service = new();
    private List<Item> _items;

    public SyncManager(List<Item> items)
    {
        _items = items;

        foreach (var item in _items)
        {
            item.Service = _service;
            AddWatchers(item);
        }
    }

    private void AddWatchers(Item item)
    {
        if (item.Rule == "upload")
        {
            item.AddLocalWatcher();
        }
        else if (item.Rule == "download")
        {
            item.AddDriveWatcher(_service);
        }
        else
        {
            item.AddLocalWatcher();
            item.AddDriveWatcher(_service);
        }
    }
}