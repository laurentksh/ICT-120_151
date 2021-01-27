# ICT-120_151

![.NET Back-End](https://github.com/laurentksh/ICT-120_151/workflows/.NET/badge.svg?branch=master)

Epsic ICT-120 &amp; 151 Exam projects (front-end &amp; back-end) written in C# and TS, using ASP.NET Core and Angular.


## Installation & Testing

Running the back-end server first is recommended.

### Back-End
Use the .NET 5 SDK to run the back-end part of the project.
Go to /src/back/ICT-151/ICT-151/ and use:

```bash
dotnet run --configuration=Release
```

#### Testing

To run tests, go to /src/back/ICT-151/ and use:

```bash
dotnet test
```


### Front-End

Go to /src/front/ICT120/, open a terminal and run the front-end part with NPM:

```bash
npm start --prod
```

or:

```bash
ng serve --prod
```

#### Testing

There are no front-end tests.

## Usage

Open your favorite browser (must not be IE, non-Chromium Edge, or any similarly outdated browser),
and go to [localhost:4200](http://localhost:4200/)

The Back-End Swagger is available at [localhost:5001/swagger](https://localhost:5001/swagger/index.html)

## Contributing
Contributions are not welcome as this is a school project.

## License
None (i.e Copyright)
