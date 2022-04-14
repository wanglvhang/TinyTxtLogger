using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lvhang.TinyTxtLogger
{

    //该服务负责定时 日志文件夹的清理，日志文件的创建以及日志的写入
    internal class LogFolderManager : ILogFolderManager
    {
        private TinyTxtLoggerOptions _currentConfig;
        private readonly IDisposable _onChangeToken;
        private string _logFolderPath;

        private static readonly Mutex mtx = new Mutex();
        public LogFolderManager(IHostEnvironment env, IOptionsMonitor<TinyTxtLoggerOptions> optionsMonitor)
        {

            this.LogFiles = new List<LogFile>();
            //设置options
            _currentConfig = optionsMonitor.CurrentValue;
            _onChangeToken = optionsMonitor.OnChange(updatedConfig => _currentConfig = updatedConfig);
            //构建日志目录
            _logFolderPath = Path.Combine(env.ContentRootPath, _currentConfig.TxtLogFolderName);

        }


        public  List<LogFile> LogFiles { get; set; }


        /// <summary>
        /// 获取目录中的日志文件信息并清理文件
        /// </summary>
        private void checkAndCleanFolder()
        {
            this.LogFiles.Clear();

            //检查目录，防止目录手动被删除的情况
            if (!Directory.Exists(_logFolderPath))
            {
                Directory.CreateDirectory(_logFolderPath);
            };

            //检查所目录文件数量 并根据配置进行清理
            var all_log_files = Directory.GetFiles(_logFolderPath, $"{_currentConfig.LogFileNamePrefix}_*", SearchOption.TopDirectoryOnly);
            foreach (var fp in all_log_files)
            {
                var logfile = LogFile.New(fp);
                if(logfile is null)
                {
                    //说明路径验证不通过，直接跳过该路径处理
                    continue;
                }

                logfile.Info = new FileInfo(fp);

                this.LogFiles.Add(logfile);
            }

            //是否需要清理文件
            if (_currentConfig.AutoCleanLogFolder)
            {
                this.LogFiles = this.LogFiles.OrderBy(l => l.TimeStr).ToList();
                if (this.LogFiles.Count > _currentConfig.ReserveLogFileCount)
                {
                    //开始清理
                    var expiredLogFiles = this.LogFiles.Take(this.LogFiles.Count - _currentConfig.ReserveLogFileCount);

                    foreach (var ef in expiredLogFiles)
                    {
                        ef.Info.Delete();
                    }

                }
            }
        }

        /// <summary>
        /// 根据参数获取当前使用的日志文件路径，该方法返回的文件路径应确保存在
        /// </summary>
        /// <param name="time"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private string getLogFilePath(DateTimeOffset time)
        {

            checkAndCleanFolder();

            //获取时间对应的logfile,并且获取其中最大的seq
            var logfile = this.LogFiles.Where(l =>
                l.Prefix == _currentConfig.LogFileNamePrefix
                && l.TimeStr == time.GetLogFileRollingTimeStr(_currentConfig.RollingInterval)
            ).OrderByDescending(l=>l.SeqNum).FirstOrDefault();

            if (logfile is null)//说明目前文件夹中没有当前时间对应的日志文件
            {
                //创建日志文件 seqnum=0
                var newlogfile = new LogFile(_currentConfig, time, 0);
                var filepath = newlogfile.GetFullPath(_logFolderPath);
                newlogfile.Info = new FileInfo(filepath);

                File.Create(filepath).Close();

                this.LogFiles.Add(newlogfile);

                return filepath;
            }
            else
            {
                //检查该日志文件的size,若已经超出限制则创建新文件
                if (logfile.Info.Length > _currentConfig.FileSizeLimit * 1024)
                {
                    //创建新的日志文件 seq + 1
                    var newlogfile = new LogFile(_currentConfig, time, logfile.SeqNum + 1);
                    var filepath = newlogfile.GetFullPath(_logFolderPath);
                    newlogfile.Info = new FileInfo(filepath);

                    File.Create(filepath).Close();

                    this.LogFiles.Add(newlogfile);

                    return filepath;

                }
                else
                {
                    return logfile.Info.FullName;
                }

            }


        }


        public void WriteLog(DateTimeOffset time, string log)
        {

            Task.Run(() =>
            {
                log += Environment.NewLine;
                var log_bytes = UnicodeEncoding.UTF8.GetBytes(log);

                mtx.WaitOne();

                var log_file_path = this.getLogFilePath(time);

                using (FileStream fs = File.Open(log_file_path, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    fs.Write(log_bytes, 0, log_bytes.Length);
                }

                mtx.ReleaseMutex();

            });

        }

    }
}
