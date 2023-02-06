namespace DriveSync;

public class Item
{
    public string Local { get; set; }
    public string Drive { get; set; }
    public string Rule { get; set; }

    private Dictionary<string, bool> _availableRules = new()
    {
        { "download", true },
        { "upload", true },
        { "sync", true }
    };

    public void OnDriveChanged()
    {
        // todo: download the file
    }

    public void OnLocalChanged(object sender, FileSystemEventArgs e)
    {
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

        if (!_availableRules.ContainsKey(Rule))
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