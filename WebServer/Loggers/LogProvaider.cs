using Microsoft.Extensions.Logging;
using System;

namespace WebServer.Log
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string path;
        public FileLoggerProvider(string _path)
        {
            path = _path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(path);
        }

        public void Dispose()
        {
        }
    }
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory,
                                        string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));
            return factory;
        }
    }
    internal static class LoggerSubsystem
    {
        internal static ILoggerFactory LoggerFactory { get; set; }
        internal static ILogger CreateLogger<t>() => LoggerFactory.CreateLogger<t>();     
    }
}
