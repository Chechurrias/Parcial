using System;
using Application.Interfaces;

namespace Infrastructure.Logging
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine("[LOG] " + DateTime.Now + " - " + message);
        }

        public void LogError(Exception ex, string message)
        {
            Console.WriteLine("[ERROR] " + DateTime.Now + " - " + message + Environment.NewLine + ex);
        }
    }
}
