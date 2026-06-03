using System;
using System.IO;
using Component1.InformationSystem.Interfaces;

namespace Component1.InformationSystem.Services
{
    public class ActivityLogger : IActivityLogger
    {
        private readonly string _logFilePath;

        public ActivityLogger(string logFilePath = "activity_log.txt")
        {
            _logFilePath = logFilePath;
        }

        public void Log(string message)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}";
            File.AppendAllText(_logFilePath, entry + Environment.NewLine);
        }
    }
}
