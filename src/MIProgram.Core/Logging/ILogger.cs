using System;
namespace MIProgram.Core.Logging
{
    public interface ILogger
    {
        void LogError(string error, ErrorLevel errorLevel);
    }

    public enum ErrorLevel
    {
        Info = 0,
        Warning = 1,
        Error = 2,
        Fatal = 3
    }

    public static class Logging
    {
        static private ILogger _instance;
        static private readonly object Padlock = new object();

        public static void SetInstance(ILogger logger)
        {
            lock (Padlock)
            {
                if (_instance != null)
                {
                    throw new Exception("Logger is already initialized, you cannot redefine it");
                }
                _instance = logger;
            }
        }

        public static ILogger Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        throw new Exception("Logger was never initialized");
                    }
                    return _instance;
                }
            }
        }
    }
}
