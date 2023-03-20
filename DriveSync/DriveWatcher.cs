namespace DriveSync;

public class DriveWatcher
{
    public DriveWatcher(DriveServiceFacade service, string path)
    {
        StartWebhook();
        var channel = service.GetWatchChannel(path);
    }

    private void StartWebhook()
    {
        
    }
}