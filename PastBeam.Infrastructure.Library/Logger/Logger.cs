using System;
using Serilog;

namespace PastBeam.Infrastructure.Logger
{
    public interface ILogger
    {
        void LogToFile(string message);
    }


    public class Logger : ILogger
    {
        static Logger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt")
                .CreateLogger();
        }

        public void LogToFile(string message)
        {
            Log.Information(message);
            Log.CloseAndFlush(); // Забезпечує коректний запис у файл
        }
    }
}
