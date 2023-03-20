namespace DriveSync;

public class Item
{
    public string LocalPath { get; set; }
    public string DrivePath { get; set; }
    public string Rule { get; set; }

    private HashSet<string> _availableRules = new()
    {
        "download",
        "upload",
        "sync"
    };
    
    private DateTime _lastLocalRead = DateTime.MinValue;

    public void AddDriveWatcher(DriveServiceFacade service)
    {
        var watcher = new DriveWatcher(service, DrivePath);
    }
    
    public void AddLocalWatcher()
    {
        var watcher = new FileSystemWatcher(Path.GetDirectoryName(LocalPath)!);
        watcher.Filter = Path.GetFileName(LocalPath);
        watcher.EnableRaisingEvents = true;
        watcher.Changed += OnLocalChanged;
        
        GC.KeepAlive(watcher);
    }

    private void OnDriveChanged()
    {
        // todo: download the file
    }

    private void OnLocalChanged(object sender, FileSystemEventArgs e)
    {
        var now = DateTime.Now;
        var lastWriteTime = File.GetLastWriteTime(LocalPath);
        
        if (e.ChangeType != WatcherChangeTypes.Changed ||
            now == lastWriteTime ||
            lastWriteTime == _lastLocalRead)
        {
            return;
        }

        _lastLocalRead = lastWriteTime;
        
        // todo: upload file to drive
    }

    public bool IsValid()
    {
        if (LocalPath is null ||
            DrivePath is null ||
            Rule is null)
        {
            return false;
        }

        if (!_availableRules.Contains(Rule))
        {
            return false;
        }

        if (!Directory.Exists(Path.GetDirectoryName(LocalPath)))
        {
            return false;
        }
        
        // todo: check Drive

        if (!File.Exists(LocalPath) &&
            Rule == "upload")
        {
            return false;
        }
        
        return true;
    }
}
