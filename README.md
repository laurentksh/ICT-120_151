# ICT-120_151
![.NET Back-End: Build - Test - Deploy](https://github.com/laurentksh/ICT-120_151/workflows/.NET%20Back-End:%20Build%20-%20Test%20-%20Deploy/badge.svg)
![Angular Front-End: Build - Deploy](https://github.com/laurentksh/ICT-120_151/workflows/Angular%20Front-End:%20Build%20-%20Deploy/badge.svg)

[Epsic](https://www.epsic.ch/) ICT-120 &amp; 151 Exam projects (front-end &amp; back-end) written in C# and TS, using ASP.NET Core and Angular.


## Installation & Testing
A "production" application instance hosted on Azure is available here:

- Back-End  : https://ict-151-back.azurewebsites.net
- Front-End : https://agreeable-forest-06fae7003.azurestaticapps.net (randomly generated subdomain...)

However, if you still want to run a local instance, there are quite a few steps required beforehand.

Running the back-end server first is recommended.

## Back-End

### Dependencies
To run this project you will need to install the .NET 5 SDK from [Microsoft](https://dotnet.microsoft.com/download).

If you want to use medias (publication images, profile pictures, etc), you will need to install Azurite, a server clone of Azure.

### (Optional) Installing Azurite
To install Azurite, you will need NPM (i.e >= Node.JS 8.0). (For more information see section [Front-End > Dependencies](#front-dependencies))

Run the following command to install Azurite:

```bash
npm install -g azurite
```

This command will install Azurite globally.

### (Optional) Running Azurite

To start Azurite, open a command prompt/PowerShell, go to the root folder of the repository and use the following command:

```bash
azurite --silent --location Azurite --debug Azurite/debug.log
```

or run the azurite.bat script present at the root foolder.

You can freely change the location as it will contain the files uploaded using the application.


### Running
Go to /src/back/ICT-151/ICT-151/ and use:

```bash
dotnet run --configuration=Release
```

### Testing
To run tests, go to /src/back/ICT-151/ and use:

```bash
dotnet test
```


## Front-End

### Dependencies
The Front-End application requires NPM (and thus Node.JS) to be installed.

Download: https://nodejs.org/en/download/

### Installation
After installing Node.JS, go to /src/front/ICT120/ and run:

```bash
npm install
```

### Running
Open a terminal and run the front-end part with NPM:

```bash
npm start
```

or:

```bash
ng serve
```

You can also add the --prod attribute to use the public back-end API.

### Testing
There are no front-end tests.


## Usage
Open your favorite browser (must not be IE, non-Chromium Edge, or any similarly outdated browser),
and go to [localhost:4200](http://localhost:4200/)

The Back-End Swagger is available at [localhost:5001/swagger](https://localhost:5001/swagger/index.html)

The public instances hosted on Azure are also available:

- Back-End  : https://ict-151-back.azurewebsites.net
- Front-End : https://agreeable-forest-06fae7003.azurestaticapps.net

## Contributing
Contributions are not welcome as this is a school project.

## License
None (i.e Copyright)
