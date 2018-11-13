using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using FileDrive = Google.Apis.Drive.v3.Data.File;

namespace BackupToGoogleDrive.Core
{
    public static class DriveServiceHelper
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        private static readonly string[] Scopes =
        {
            DriveService.Scope.Drive,
            DriveService.Scope.DriveFile,
            DriveService.Scope.DriveAppdata
        };

        public static DriveService Create(OptionsBase options)
        {
            if (string.IsNullOrEmpty(options.ServiceAccountCredentialFile))
                throw new Exception("Path to the service account credentials file is required.");

            if (!File.Exists(options.ServiceAccountCredentialFile))
                throw new Exception("The service account credentials file does not exist at: " + options.ServiceAccountCredentialFile);

            // For Json file
            if (Path.GetExtension(options.ServiceAccountCredentialFile).ToLower() == ".json")
            {
                Console.WriteLine("Initializing connection...");


                GoogleCredential credential;

                using (var stream = new FileStream(options.ServiceAccountCredentialFile, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
                }

                // Create the  Analytics service.
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = options.ApplicationName
                });

                Console.WriteLine("Connection OK");

                return service;
            }

            throw new Exception("Unsupported Service accounts credentials.");
        }

        #region Files

        public static async Task<IList<FileDrive>> ListFilesAsync(this DriveService service, string name = null, FileDrive folder = null, DateTime? purgeTime = null)
        {
            #region Request

            var request = service.Files.List();
            request.PageSize = 50;
            request.Fields = "files(id,mimeType,name,shared),nextPageToken";

            #region Query

            var q = new List<string>(2);

            if (!string.IsNullOrEmpty(name)) q.Add($"name = '{name}'");
            if (folder != null) q.Add($"parents in '{folder.Id}'");
            if (purgeTime.HasValue) q.Add($"modifiedTime <= '{purgeTime.Value:yyyy-MM-ddTHH:mm:ss}'");

            request.Q = string.Join(" and ", q);

            #endregion Query

            #endregion Request

            var response = await request.ExecuteAsync();

            return response.Files;
        }

        public static async Task DeleteFileAsync(this DriveService service, FileDrive file)
        {
            var request = service.Files.Delete(file.Id);

            Console.WriteLine("File '{0}' deleting...", file.Name);

            await request.ExecuteAsync();

            Console.WriteLine("File '{0}' deleted!", file.Name);
        }

        #endregion Files

        #region Folder

        public static async Task<FileDrive> CreateFolderAsync(this DriveService service, string folderName)
        {
            var request = service.Files.Create(new FileDrive
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder"
            });

            Console.WriteLine("Folder '{0}' creating...", folderName);

            var folder = await request.ExecuteAsync();

            Console.WriteLine("Folder '{0}' created!", folderName);

            return folder;
        }

        public static async Task<FileDrive> CreateFolderIfNotExistsAsync(this DriveService service, string folderName)
        {
            FileDrive file = null;

            var files = await ListFilesAsync(service, folderName, null);

            if (files != null)
            {
                file = files.FirstOrDefault();
            }

            return file ?? await CreateFolderAsync(service, folderName);
        }

        #endregion Folder

        #region Upload

        public static async Task UploadFileAsync(this DriveService service, string path, string mimeType, FileDrive folder)
        {
            var file = new FileDrive
            {
                Name = Path.GetFileName(path),
                Parents = folder != null ? new List<string> { folder.Id } : null
            };

            var request = service.Files.Create(file, File.OpenRead(path), mimeType);

            request.ResponseReceived += OnUploadResponseReceived;
            request.ProgressChanged += OnUploadProgressChanged;

            Console.WriteLine("File '{0}' uploading...", file.Name);

            await request.UploadAsync();
        }

        private static void OnUploadProgressChanged(IUploadProgress progress)
        {
            if (progress.Exception != null)
            {
                throw progress.Exception;
            }

            if (progress.Status == UploadStatus.Uploading)
            {
                Console.WriteLine("{0} ({1})", progress.Status, progress.BytesSent);
            }
        }

        private static void OnUploadResponseReceived(FileDrive file)
        {
            Console.WriteLine("File '{0}' uploaded!", file.Name);
        }

        #endregion Upload
    }
}