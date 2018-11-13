using BackupToGoogleDrive.Core;
using CommandLine;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BackupToGoogleDrive.CLI
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<UploadOptions, PurgeOptions>(args)
                .MapResult(
                    (UploadOptions options) => UploadAsync(options).GetAwaiter().GetResult(),
                    (PurgeOptions options) => PurgeAsync(options).GetAwaiter().GetResult(),
                    _ => 1);

#if DEBUG
            Console.ReadKey();
#endif

            return result;
        }

        private static async Task<int> UploadAsync(UploadOptions options)
        {
            try
            {
                // Create service
                var service = DriveServiceHelper.Create(options);

                // Get folder
                var folder = await service.CreateFolderIfNotExistsAsync(options.SharedFolder);

                foreach (var filePath in options.Files)
                {
                    if (options.RemoveDuplicateFiles)
                    {
                        // Get files by same name
                        var files = await service.ListFilesAsync(Path.GetFileName(filePath), folder);

                        if (files != null)
                        {
                            foreach (var file in files)
                            {
                                // Delete files by same name
                                await service.DeleteFileAsync(file);
                            }
                        }
                    }

                    // Upload file
                    await service.UploadFileAsync(filePath, MimeTypeMap.GetMimeType(Path.GetExtension(filePath)), folder);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);

                return 1;
            }
        }

        private static async Task<int> PurgeAsync(PurgeOptions options)
        {
            try
            {
                // Create service
                var service = DriveServiceHelper.Create(options);

                // Get folder
                var folder = await service.CreateFolderIfNotExistsAsync(options.SharedFolder);

                foreach (var file in await service.ListFilesAsync(null, folder, DateTime.UtcNow.AddDays(-options.Days)))
                {
                    // Delete file
                    await service.DeleteFileAsync(file);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);

                return 1;
            }
        }
    }
}