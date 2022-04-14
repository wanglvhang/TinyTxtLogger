using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System;


namespace Lvhang.TinyTxtLogger
{
    public static class TinyTxtLoggerExtensions
    {
        public static ILoggingBuilder AddTinyTxt(this ILoggingBuilder builder)
        {

            builder.AddConfiguration();

            builder.Services.AddSingleton<ILogFolderManager, LogFolderManager>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TinyTxtLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<TinyTxtLoggerOptions, TinyTxtLoggerProvider>(builder.Services);
            return builder;

        }


        public static ILoggingBuilder AddTinyTxt(this ILoggingBuilder builder, Action<TinyTxtLoggerOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException("configure");
            }

            builder.AddTinyTxt();
            builder.Services.Configure(configure);
            return builder;

        }


        public static IHostBuilder BindTinyTxtLoggerOptions(this IHostBuilder builder)
        {

            builder.ConfigureServices((context, services) => {

                services.Configure<TinyTxtLoggerOptions>(context.Configuration.GetSection("Logging").GetSection("TinyTxt").GetSection("Options"));

            });




            return builder;
        }

    }
}
