using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Utils
{
    public static class Logger
    {
        private static readonly object _lock = new();
        private static readonly string _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "elevator_log.txt");

        public static void Log(string message)
        {
            lock (_lock)
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
        }

        public static void LogError(string error)
        {
            Log("ERROR: " + error);
        }

        public static void LogDebug(string debug)
        {
            Log("DEBUG: " + debug);
        }
        public static void LogInfo(string debug)
        {
            Log("INFO: " + debug);
        }
    }
}
