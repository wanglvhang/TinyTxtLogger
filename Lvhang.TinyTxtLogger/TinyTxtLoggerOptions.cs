using Microsoft.Extensions.Configuration;
using System;

namespace Lvhang.TinyTxtLogger
{
    public class TinyTxtLoggerOptions
    {

        /// <summary>
        /// 保存日志文件的文件夹名称
        /// </summary>
        public string TxtLogFolderName { get; set; } = "logs";


        /// <summary>
        /// 基于时间滚动文件时的模式
        /// </summary>
        public eRollingInterval RollingInterval { get; set; } = eRollingInterval.Day;


        /// <summary>
        /// 单个日志文件大小限制,单位KB，默认10MB
        /// </summary>
        public long FileSizeLimit { get; set; } = 102400;


        /// <summary>
        /// 日志文件名称前缀
        /// </summary>
        public string LogFileNamePrefix { get; set; } = "log";


        /// <summary>
        /// 是否自动清理日志文件夹
        /// </summary>
        public bool AutoCleanLogFolder { get; set; } = false;


        /// <summary>
        /// 自动日志清理时，要保留的日志文件数量，有限保留最近创建的
        /// </summary>
        public int ReserveLogFileCount { get; set; } = 30;


    }
}
