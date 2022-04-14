using NugetPackageTest;
using Lvhang.TinyTxtLogger;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((context, logging) =>
    {

        logging.ClearProviders();
        logging.AddTinyTxt();
        //logging.AddTinyTxt(options =>
        //{
        //    options.TxtLogFolderName = "logs"; //log folder name
        //    options.LogFileNamePrefix = "log"; //log file prefix
        //    options.RollingInterval = eRollingInterval.Day; //log file rolling interval
        //    options.FileSizeLimit = 1024 * 10;// size limit of single file
        //    options.AutoCleanLogFolder = false; // if log file in folder over the limit if clean the folder 
        //    options.ReserveLogFileCount = 30; //log file reserve when clean the log folder

        //});
        //logging.AddConsole();
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
