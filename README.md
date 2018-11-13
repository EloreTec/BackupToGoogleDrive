# Backup to Google Drive

## Usage

- Download dist file
  - bkp-gdrive-cli461.zip to .NET Framework 4.6.1 or newest (only Windows)
  - bkp-gdrive-cli.zip to .NET Core 2.1.1 or newest
- Create new project in http://console.developers.google.com
- Enable Google API
- Create service account credential
- Download service account credential json file (renamed service-account-credential.json) in dist folder
- Execute commands

### Upload

```
[4.6.1] bkp-gdrive-cli.exe upload -s [shared-folder] -f "[file]"
[core]  dotnet bkp-gdrive-cli.dll upload -s [shared-folder] -f "[file]"

  -s, --shared-folder                      Required. Set shared folder to
                                           backup files in Google Drive.

  -f, --files                              Required. Set files path.

  -r, --remove-duplicate                   (Default: true) Set to remove
                                           duplicate files.

  -c, --service-account-credential-file    (Default:
                                           service-account-credential.json) Set
                                           service account credentials file to
                                           connect Google Drive Api.

  -n, --aplication-name                    (Default: Backup to Google Drive)
                                           Set application name.
```

-------------------

### Purge

```
[4.6.1] bkp-gdrive-cli.exe upload -s [shared-folder] -d [days]
[core] dotnet bkp-gdrive-cli.dll upload -s [shared-folder] -d [days]

  -s, --shared-folder                      Required. Set shared folder to purge
                                           files in Google Drive.

  -d, --days                               Required. Set days to purge files in
                                           Google Drive, by modification date.

  -r, --remove-duplicate                   (Default: true) Set to remove
                                           duplicate files.

  -c, --service-account-credential-file    (Default:
                                           service-account-credential.json) Set
                                           service account credentials file to
                                           connect Google Drive Api.

  -n, --aplication-name                    (Default: Backup to Google Drive)
                                           Set application name.

  --help                                   Display this help screen.

  --version                                Display version information.
```
