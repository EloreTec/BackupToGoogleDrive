using CommandLine;

namespace BackupToGoogleDrive.Core
{
    public abstract class OptionsBase
    {
        [Option('c', "service-account-credential-file", Required = false, HelpText = "Set service account credentials file to connect Google Drive Api.", Default = "service-account-credential.json")]
        public string ServiceAccountCredentialFile { get; set; }

        [Option('n', "aplication-name", Required = false, HelpText = "Set application name.", Default = "Backup to Google Drive")]
        public string ApplicationName { get; set; }
    }
}