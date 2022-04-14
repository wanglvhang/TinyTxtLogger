using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Lvhang.TinyTxtLogger
{

    //[UnsupportedOSPlatform("browser")]
    [ProviderAlias("TinyTxt")]
    internal class TinyTxtLoggerProvider : ILoggerProvider
    {

        private readonly ConcurrentDictionary<string, TinyTxtLogger> _loggers = new ConcurrentDictionary<string, TinyTxtLogger>(StringComparer.OrdinalIgnoreCase);

        private ILogFolderManager _logFolderManager;
        //需要注入配置
        public TinyTxtLoggerProvider(ILogFolderManager logFolderManager)
        {
            _logFolderManager = logFolderManager;
        }


        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new TinyTxtLogger(name, _logFolderManager));
        }


        public void Dispose()
        {
            _loggers.Clear();

        }
    }
}
