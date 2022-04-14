using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lvhang.TinyTxtLogger
{
    internal class TinyTxtLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly ILogFolderManager _logFolderManager;

        public TinyTxtLogger([NotNull] string categoryName, ILogFolderManager logFolderManager)
        {
            _categoryName = categoryName;
            _logFolderManager = logFolderManager;
        }


        public IDisposable BeginScope<TState>(TState state)
        {
            return default!;
        }


        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }


        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            //生成日志字符串
            StringBuilder sb = new StringBuilder();
            var now = DateTimeOffset.Now;
            //meta info line
            //infomation|eventid|time|categoryname
            //ISO 8601 time format
            sb.AppendLine($"{logLevel.ToString().ToLower()}|{eventId.Id}|{now.ToString("o")}|{_categoryName}");
            //log info lines
            sb.Append(formatter(state, exception));

            _logFolderManager.WriteLog(now, sb.ToString());

        }



    }
}
