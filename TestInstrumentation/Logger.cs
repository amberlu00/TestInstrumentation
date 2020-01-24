using System;
using Serilog;
using System.Collections.Generic;

namespace TestInstrumentation
{
    public class Logger
    {

        private string appType;
        /**
         * Class for the logger.
         * @param appType: "Producer" or "Consumer". Used in file naming.
         */
        public Logger(string appType)
        {
            this.appType = appType;
            Log.Logger = new LoggerConfiguration().
                WriteTo.File(string.Concat(appType, ".txt"),
                rollingInterval: RollingInterval.Day).CreateLogger();

            Dictionary<string, string> keyToLog = new Dictionary<string, string>();
        }

        /**
         * Logs a status.
         * @param info: status to log
         */
        public void LogStatus(string info) {
            Log.Information(string.Concat("{From}: ", info), appType);
        }

        /**
         * Logs an error.
         * @param info: error to log
         */
        public void LogError(string info)
        {
            Log.Error(string.Concat("{From}: ", info), appType);
        }

        public void LogFatal(string info)
        {
            Log.Fatal(string.Concat("{From}: ", info), appType);
        }

        public void LogWarning(string info)
        {
            Log.Warning(string.Concat("{From}: ", info), appType);
        }
    }
}
