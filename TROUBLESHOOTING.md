# Troubleshooting
This file aims to help troubleshoot issues when running a local instance of the project.

## Back-End
Back-End common issues.
The titles below are read from the console logging of the application.

### Any.Get Error 500 / ... SQLite Error 1: 'no such table: %table%'
This error happens when the database has not been migrated beforehand.

To resolve this issue, follow the steps in [README.md](readme.md#migrating-the-database) on how to run the application.

### Media.Get Error 500 / No connection could be established (127.0.0.1:10000)
This error means the back-end could not reach Azurite (the Azure Blob Storage emulator).

To resolve this issue, follow the steps in [README.md](readme.md#optional-installing-azurite) on how to run Azurite.

## Front-End
Front-End common issues.
Titles below are read from the DevTools console of the web browser.

### Error codes
If you see a message
### Failed to load resource: net::ERR_CERT_AUTHORITY_INVALID / An error occured... (undefined)
This error means you haven't setup correctly the local certificate for HTTPS.

To resolve this issue, you can either try to add correctly the development certificate by using the following command:

```bash
dotnet dev-certs https --trust
```

or if this doens't work, open your browser, go to https://localhost:5001/swagger.

There should be a warning page from your browser saying "Your connection isn't private".
Click "Advanced" and "Continue to localhost (unsecure)".

The steps to ignore the security warning may vary depending on your browser.