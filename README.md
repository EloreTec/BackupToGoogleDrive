# Backup to Google Drive

## Usage

Upload

```
bkp-gdrive-cli.exe upload -s [shared-folder] -f "[file]"

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

Purge

```
bkp-gdrive-cli.exe upload -s [shared-folder] -d [days]

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
