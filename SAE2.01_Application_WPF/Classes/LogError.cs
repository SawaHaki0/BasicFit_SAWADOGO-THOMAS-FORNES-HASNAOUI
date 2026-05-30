using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SAE2._01_Application_WPF.Classes
{
    public class LogError
    {
        public static void Log(Exception ex, string msg)
        {
            string logFile = "error.log";
            string content = $"{DateTime.Now}:{msg}\n {ex.Message}\n{ex.StackTrace}\n";
            try
            {
                File.AppendAllText(logFile, content);
            }
            catch
            { }
        }
    }
}
