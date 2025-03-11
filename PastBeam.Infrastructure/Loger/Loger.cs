using System;
using Serilog;

namespace PastBeam.Infrastructure.Logger
{
    public static class Logger
    {
        static Logger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt")
                .CreateLogger();
        }

        public static void LogToFile(string message)
        {
            Log.Information(message);
            Log.CloseAndFlush(); // Забезпечує коректний запис у файл
        }
    }
}
