using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lvhang.TinyTxtLogger
{
    public static class HelperExtensions
    {

        public static string GetLogFileRollingTimeStr(this DateTimeOffset time, eRollingInterval rollingInterval)
        {
            switch (rollingInterval)
            {
                case eRollingInterval.Infinite:
                    return "infinite";
                case eRollingInterval.Hour:
                    return time.ToString("yyyyMMddHH");
                case eRollingInterval.Day:
                    return time.ToString("yyyyMMdd");
                case eRollingInterval.Mounth:
                    return time.ToString("yyyyMM");
                case eRollingInterval.Year:
                    return time.ToString("yyyy");
                default:
                    throw new Exception("impossible!!");
            }
        }


    }
}
