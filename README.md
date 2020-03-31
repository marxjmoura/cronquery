# CronQuery

Lightweight job runner for ASP.NET Core.

[![CircleCI](https://circleci.com/gh/marxjmoura/cronquery.svg?style=shield)](https://circleci.com/gh/marxjmoura/cronquery)
[![codecov](https://codecov.io/gh/marxjmoura/cronquery/branch/master/graph/badge.svg)](https://codecov.io/gh/marxjmoura/cronquery)
[![NuGet Version](https://img.shields.io/nuget/v/cronquery.svg)](https://img.shields.io/nuget/v/cronquery.svg)
[![NuGet Downloads](https://img.shields.io/nuget/dt/cronquery.svg)](https://www.nuget.org/packages/cronquery)

## Changelog

The changes for each release are documented in the [release notes](https://github.com/marxjmoura/cronquery/releases).

## Installation

Package Manager (Visual Studio):

```
Install-Package CronQuery
```

.NET CLI:

```
dotnet add package CronQuery
```

## Creating a job

Jobs are created by implementing the interface `IJob`.

```c#
public class MyJob : IJob
{
    public async Task RunAsync()
    {
        // Do your magic
    }
}
```

## Registering a job

Jobs are registered in the app's `Startup` class.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddCronQuery(Configuration.GetSection("CronQuery"));

    services.AddTransient<MyFirstJob>();
    services.AddTransient<MySecondJob>();
    services.AddSingleton<MyThirdJob>();
}
```

> Jobs are registered using the ASP.NET Core dependency injection. This means that is possible to use dependency injection in your jobs.

## Setting up a job

Schedule your jobs using [cron expressions](CRON.md) of six fields to a specific timezone (UTC is default). Save the configuration in your `appsettings.json` like the example below:

- `MyFirstJob`: Runs every second on every day, except Sunday.

- `MySecondJob`: Runs every day at 2:00 A.M.

- `MyThirdJob`: Runs every second between 2:00 P.M. and 6:59 P.M. only on Saturday every 15 days.

```json
{
  "CronQuery": {
    "Running": true,
    "TimeZone": "E. South America Standard Time",
    "Jobs": [
      {
        "Name": "MyFirstJob",
        "Running": true,
        "Cron": "* * * * * 1-6"
      },
      {
        "Name": "MySecondJob",
        "Running": true,
        "Cron": "0 0 2 * * *"
      },
      {
        "Name": "MyThirdJob",
        "Running": true,
        "Cron": "* * 14-18 * * 6/15"
      }
    ]
  }
}
```

Whenever you save the `appsettings.json` CronQuery immediately assumes the new configuration.

| Property                   | Description                                                                |
|----------------------------|----------------------------------------------------------------------------|
| `CronQuery.Running`        | Turn on (`true`) or turn off (`false`) the CronQuery runner.               |
| `CronQuery.TimeZone`       | System time zone ID or a custom UTC offset, e.g. `UTC-03:00`, `UTC+01:00`. |
| `CronQuery.Jobs[].Name`    | Job class name.                                                            |
| `CronQuery.Jobs[].Running` | Turn on (`true`) or turn off (`false`) the job.                            |
| `CronQuery.Jobs[].Cron`    | Cron expression that triggers the job.                                     |

## Bugs and features

Please, fell free to [open a new issue](https://github.com/marxjmoura/cronquery/issues/new) on GitHub.

## License

[MIT](https://github.com/marxjmoura/cronquery/blob/master/LICENSE)

Copyright (c) 2018-present, [Marx J. Moura](https://github.com/marxjmoura)
