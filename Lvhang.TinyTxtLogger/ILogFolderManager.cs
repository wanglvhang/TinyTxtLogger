using System;
using System.Collections.Generic;
using System.Text;

namespace Lvhang.TinyTxtLogger
{
    internal interface ILogFolderManager
    {

        void WriteLog(DateTimeOffset time, string log);

    }
}
