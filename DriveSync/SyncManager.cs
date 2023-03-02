namespace DriveSync;

public class SyncManager
{
    private DriveServiceFacade _service = new();
    private List<Item> _items;

    public SyncManager(List<Item> items)
    {
        _items = items;
        
        AddWatchers();
    }

    private void AddWatchers()
    {
        foreach (var item in _items)
        {
            if (item.Rule == "upload")
            {
                item.AddLocalWatcher();
            }
            else if (item.Rule == "download")
            {
                item.AddDriveWatcher();
            }
            else
            {
                item.AddLocalWatcher();
                item.AddDriveWatcher();
            }
        }
    }
}