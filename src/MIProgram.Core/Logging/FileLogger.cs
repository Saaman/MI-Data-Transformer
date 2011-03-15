using System.IO;
using System.Configuration;
using System;

namespace MIProgram.Core.Logging
{
    public class FileLogger : ILogger
    {
        readonly string _logFilePath;
        private readonly int _minErrorLevelLogged = Convert.ToInt32(ConfigurationManager.AppSettings["MinErrorLevelLogged"]);

        public FileLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }
        }

        public void LogError(string error, ErrorLevel errorLevel)
        {
            if ((int)errorLevel >= _minErrorLevelLogged)
            {
                using (var sw = new StreamWriter(_logFilePath, true))
                {
                    sw.WriteLine(string.Format("{0} : {1}", errorLevel, error));
                }
            }
        }
    }
}
