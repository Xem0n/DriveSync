namespace DriveSync;

public class Item
{
    public string Local { get; set; }
    public string Drive { get; set; }
    public string Rule { get; set; }

    private HashSet<string> _availableRules = new()
    {
        "download",
        "upload",
        "sync"
    };
    
    private DateTime _lastLocalRead = DateTime.MinValue;

    public void AddDriveWatcher()
    {
        
    }
    
    public void AddLocalWatcher()
    {
        var watcher = new FileSystemWatcher(Path.GetDirectoryName(Local)!);
        watcher.Filter = Path.GetFileName(Local);
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
        var lastWriteTime = File.GetLastWriteTime(Local);
        
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
        if (Local is null ||
            Drive is null ||
            Rule is null)
        {
            return false;
        }

        if (!_availableRules.Contains(Rule))
        {
            return false;
        }

        if (!Directory.Exists(Path.GetDirectoryName(Local)))
        {
            return false;
        }
        
        // todo: check Drive

        if (!File.Exists(Local) &&
            Rule == "upload")
        {
            return false;
        }
        
        return true;
    }
}
