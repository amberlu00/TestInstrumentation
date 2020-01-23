using System;
using Serilog;

namespace TestInstrumentation
{
    public class Logger
    {
        /**
         * Class for the logger.
         * @param appType: "Producer" or "Consumer". Used in file naming.
         */
        public Logger(string appType)
        {
            Log.Logger = new LoggerConfiguration().
                WriteTo.File(string.Concat(appType, ".txt"),
                rollingInterval: RollingInterval.Day).CreateLogger();
        }

        /**
         * Logs a status, denoted by [INF].
         * @param info: status to log
         */
        public void LogStatus(string info) {
            Log.Information(string.Concat("{Now}", info, "\n"), DateTime.Now);
        }

        /**
         * Logs an error.
         * @param info: error to log
         */
        public void LogError(string info)
        {
            Log.Error(string.Concat("{Now}", info, "\n"), DateTime.Now);
        }
    }
}
