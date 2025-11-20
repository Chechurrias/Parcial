using System;

namespace Application.Interfaces
{
    public interface ILogger
    {
        void Log(string message);
        void LogError(Exception ex, string message);
    }
}
