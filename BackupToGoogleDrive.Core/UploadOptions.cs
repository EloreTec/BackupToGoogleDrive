using System.Collections.Generic;
using CommandLine;

namespace BackupToGoogleDrive.Core
{
    [Verb("upload", HelpText = "Upload files to Google Drive.")]
    public class UploadOptions : OptionsBase
    {
        [Option('s', "shared-folder", Required = true, HelpText = "Set shared folder to backup files in Google Drive.")]
        public string SharedFolder { get; set; }

        [Option('f', "files", Required = true, HelpText = "Set files path.")]
        public IEnumerable<string> Files { get; set; }

        [Option('r', "remove-duplicate", Required = false, HelpText = "Set to remove duplicate files.", Default = true)]
        public bool RemoveDuplicateFiles { get; set; }
    }
}