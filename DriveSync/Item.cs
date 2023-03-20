namespace DriveSync;

public class Item
{
    private DriveServiceFacade? _service;
    
    public string LocalPath { get; set; }
    public string DriveFileId { get; set; }
    public string Rule { get; set; }
    public DriveServiceFacade Service
    {
        set => _service = value;
    }

    private HashSet<string> _availableRules = new()
    {
        "download",
        "upload",
        "sync"
    };
    
    private DateTime _lastLocalRead = DateTime.MinValue;

    public void AddDriveWatcher(DriveServiceFacade service)
    {
        var watcher = new DriveWatcher(service, DriveFileId);
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
        if (!isFirstTime(e))
        {
            return;
        }
       
        Upload();
    }

    private bool isFirstTime(FileSystemEventArgs e)
    {
        var now = DateTime.Now;
        var lastWriteTime = File.GetLastWriteTime(LocalPath);
        
        if (e.ChangeType != WatcherChangeTypes.Changed ||
            now == lastWriteTime ||
            lastWriteTime == _lastLocalRead)
        {
            return false;
        }

        _lastLocalRead = lastWriteTime;

        return true;
    }

    private void Upload()
    {
        var driveFile = new Google.Apis.Drive.v3.Data.File();
        driveFile.Id = DriveFileId;

        using (var file = File.OpenRead(LocalPath))
        {
            
        }
    }

    public bool IsValid()
    {
        if (LocalPath is null ||
            DriveFileId is null ||
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
