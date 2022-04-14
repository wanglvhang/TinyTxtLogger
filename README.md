# TinyTxtLogger 

## summary

this logger tool is fully based on Microsoft.Extensons, you can use it in .NET Worker Service or ASP.NET Core project. if you are working on those project and looking for a logger tool that save logs to local txt file, this is the one you need.

it's very easy to use this tool, you only need one line of code to add this tool to your project: `logging.AddTinyTxt();`。

## installition

you can include this tool from NuGet with name Lvhang.TinyTxtLogger.

```
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((context, logging) =>

    {

        logging.ClearProviders();

        logging.AddTinyTxt();//add TinyTxtLogger

    })

    .ConfigureServices(services =>

    {

        services.AddHostedService<Worker>();

    })

    .Build();
```

## configuration
there's two way to configure the tool

**way 1 with code:**

```
logging.AddTinyTxt(options =>

        {

            options.TxtLogFolderName = "logs"; //log folder name

            options.LogFileNamePrefix = "log"; //log file prefix

            options.RollingInterval = eRollingInterval.Day; //log file rolling interval

            options.FileSizeLimit = 10240;// size limit of single file

           options.AutoCleanLogFolder = false; // if log file in folder over the limit if clean the folder

            options.ReserveLogFileCount = 30; //log file reserve when clean the log folder

        });
```   

**way 2 with appsetting.json file:**

  ```
{

  "Logging": {

    "LogLevel": {

      "Default": "Trace",

      "Microsoft.Hosting.Lifetime": "Trace"

    },

    "TinyTxt": {

      "LogLevel": {

        "Default": "Trace",

        "Microsoft.Hosting.Lifetime": "Trace"

      },

      "Options": {

        "TxtLogFolderName": "logs",

        "LogFileNamePrefix": "log",

        "RollingInterval": "Year",  // Hour,Day,Month,Year,Infinite

        "FileSizeLimit": 10240,  //in KB

        "AutoCleanLogFolder": false,

        "ReserveLogFileCount": 30

      }

    }

  }

}
``` 

after you complete your configuration in appsetting.json, you need add one line to your code to bind your configuration:

``` 
IHost host = Host.CreateDefaultBuilder(args)

    .BindTinyTxtLoggerOptions() //this line to bind configuration

    .ConfigureLogging((context, logging) =>

    {

        logging.ClearProviders();

        logging.AddTinyTxt(;

    })

    .ConfigureServices(services =>

    {
        services.AddHostedService<Worker>();

    })

    .Build();
``` 

and that's almost every this of this tool, except that this project provide a tool to help you to view log file more friendly.

## log viewer tool
log viewer tool is a single html page app, with this tool you can search logs and view log file statistics charts as below screenshot shows.
<br>
<br>
![image](https://user-images.githubusercontent.com/936437/163235586-7103cc28-7a7c-4a6c-9b06-d4f83bb0922d.png)


