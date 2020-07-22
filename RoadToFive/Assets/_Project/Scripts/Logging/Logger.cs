using UnityEngine;

namespace _Project.Scripts.Logging
{
    public interface ILogger
    {
        void Log(string message);
    }
    
    public class DebugLogger : ILogger
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }
    }

    public static class Logger
    {
        private static readonly ILogger _logger = new DebugLogger();

        public static void Info(string message) => _logger.Log(message);
        public static void Warning(string message) => _logger.Log("<color=#ffff00>" + message + "</color>");
        public static void Error(string message) => _logger.Log("<color=#ff0000>" + message + "</color>");
    }
}