using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Lvhang.TinyTxtLogger
{
    internal class LogFile
    {

        public static LogFile New([NotNull] string filepath)
        {
            var filename = Path.GetFileNameWithoutExtension(filepath);

            var paras = filename.Split('_', StringSplitOptions.RemoveEmptyEntries);

            var logFile = new LogFile();

            try
            {
                logFile.Prefix = paras[0];
                logFile.TimeStr = paras[1];
                logFile.SeqNum = Convert.ToInt32(paras[2]);

                if (logFile.TimeStr == "infinite")
                {
                    logFile.RollingInterval = eRollingInterval.Infinite;
                }
                else
                {
                    switch (logFile.TimeStr.Length)
                    {
                        case 10:
                            logFile.RollingInterval = eRollingInterval.Hour;
                            break;
                        case 8:
                            logFile.RollingInterval = eRollingInterval.Day;
                            break;
                        case 6:
                            logFile.RollingInterval = eRollingInterval.Mounth;
                            break;
                        case 4:
                            logFile.RollingInterval = eRollingInterval.Year;
                            break;
                    }
                }

                return logFile;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private LogFile()
        {

        }


        public LogFile(TinyTxtLoggerOptions options,DateTimeOffset time,int seqnum)
        {

            if (string.IsNullOrWhiteSpace(options.LogFileNamePrefix))
            {
                options.LogFileNamePrefix = "log";
            }
            this.Prefix = options.LogFileNamePrefix;

            this.RollingInterval = options.RollingInterval;
            this.SeqNum = seqnum;
            this.TimeStr = time.GetLogFileRollingTimeStr(options.RollingInterval);


            this.FileName = $"{options.LogFileNamePrefix}_{this.TimeStr}_{seqnum}.log";
        }


        public eRollingInterval RollingInterval { get; private set; }


        public string TimeStr { get; private set; }


        public int SeqNum { get; private set; }


        public string Prefix { get; private set; }


        public string FileName { get;private set; }


        public FileInfo Info { get; set; }


        public string GetFullPath(string logFolderPath)
        {
            return Path.Combine(logFolderPath, this.FileName);
        }


    }
}
