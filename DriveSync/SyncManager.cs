namespace DriveSync;

public class SyncManager
{
    private DriveServiceFacade _service = new();
    private List<Item> _items;

    public SyncManager(List<Item> items)
    {
        _items = items;
        
        AddDriveWatch();
        AddLocalWatch();
    }

    private void AddDriveWatch()
    {
        foreach (var item in GetWatchItems("download"))
        {
            // todo: add drive watcher
        }
    }

    private void AddLocalWatch()
    {
        foreach (var item in GetWatchItems("upload"))
        { 
            using var watcher = new FileSystemWatcher(item.Local);
            watcher.Changed += item.OnLocalChanged;
        }
    }

    private IEnumerable<Item> GetWatchItems(string rule)
    {
        return _items.Where(item => item.Rule == rule || item.Rule == "sync");
    }
}