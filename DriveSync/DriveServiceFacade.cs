using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using File = Google.Apis.Drive.v3.Data.File;

namespace DriveSync;

public class DriveServiceFacade
{
    private DriveService _service;
    
    public DriveServiceFacade()
    {
        Task.Run(InitializeService).Wait();
    }

    public Channel GetWatchChannel(string fileId)
    {
        var channel = _service.Files.Watch(new Channel
        {
            // todo: fill it
        }, fileId);
        
        return channel.Execute();
    }

    public void UploadFile(File driveFile, FileStream file)
    {
        var request = _service.Files.Create(driveFile, file, driveFile.MimeType);
        request.Upload();
    }

    private async Task InitializeService()
    {
        UserCredential credential = await GetCredentials();

        _service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "drivesync"
        });
    }

    private async Task<UserCredential> GetCredentials()
    {
        using (FileStream fs = new("client_secrets.json", FileMode.Open, FileAccess.Read))
        {
            return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(fs).Secrets,
                new[] { DriveService.Scope.Drive },
                "user", CancellationToken.None, new FileDataStore("Drive.Access")
            );
        }
    }
}