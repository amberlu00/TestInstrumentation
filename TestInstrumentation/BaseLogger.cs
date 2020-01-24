﻿using Serilog;

namespace TestInstrumentation
{
    public class BaseLogger
    {

        /**
         * Class for the logger.
         * @param appType: "Producer" or "Consumer". Used in file naming.
         */
        public BaseLogger(string appType)
        {
            //Log.Logger = new LoggerConfiguration().
            //    WriteTo.File(string.Concat(appType, ".txt"),
            //    rollingInterval: RollingInterval.Day).CreateLogger();

            Log.Logger = new LoggerConfiguration().
                WriteTo.ApplicationInsights(
                new Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration("db8b2b4f-1aee-4295-a1bd-e74e8d109d09"),
                TelemetryConverter.Traces)
                .CreateLogger();
        }

        /**
         * Logs a status.
         * @param info: status to log
         */
        public void LogStatus(string info)
        {
            Log.Information(info);
        }
        /**
         * Logs an error.
         * @param info: error to log
         */
        public void LogError(string info)
        {
            Log.Error(info);
        }

        /**
         * Logs fatal errors.
         */
        public void LogFatal(string info)
        {
            Log.Fatal(info);
        }
        /**
         * Logs warnings.
         */
        public void LogWarning(string info)
        {
            Log.Warning(info);
        }

        public void LogDebug(string info)
        {
            Log.Debug(info);
        }
    }
}
