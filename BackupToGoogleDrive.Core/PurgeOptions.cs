using CommandLine;

namespace BackupToGoogleDrive.Core
{
    [Verb("purge", HelpText = "Purge folder in Google Drive.")]
    public class PurgeOptions : OptionsBase
    {
        [Option('s', "shared-folder", Required = true, HelpText = "Set shared folder to purge files in Google Drive.")]
        public string SharedFolder { get; set; }

        [Option('d', "days", Required = true, HelpText = "Set days to purge files in Google Drive, by modification date.")]
        public int Days { get; set; }

        [Option('r', "remove-duplicate", Required = false, HelpText = "Set to remove duplicate files.", Default = true)]
        public bool RemoveDuplicateFiles { get; set; }
    }
}