using Serilog;

namespace TestInstrumentation
{
    public class BaseLogger
    {

        private Microsoft.ApplicationInsights.TelemetryClient telemetryClient { get; set; }

        /**
         * Class for the logger.
         * @param appType: "Producer" or "Consumer". Used in file naming.
         */
        public BaseLogger(string appType)
        {
            if (System.Environment.GetEnvironmentVariable("instkey") != null)
            {
                telemetryClient =
                new Microsoft.ApplicationInsights.TelemetryClient()
                {
                    InstrumentationKey = System.Environment.GetEnvironmentVariable("instkey")
                };
                Log.Logger = new LoggerConfiguration().
                    WriteTo.ApplicationInsights(
                    telemetryClient,
                    TelemetryConverter.Traces)
                    .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration().
                    WriteTo.File(string.Concat(appType, ".txt"),
                    rollingInterval: RollingInterval.Day).CreateLogger();
            }
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
            telemetryClient.Flush();
        }

        /**
         * Logs fatal errors.
         */
        public void LogFatal(string info)
        {
            Log.Fatal(info);
            telemetryClient.Flush();
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

        /**
         * Flush the log to Azure, bypassing the delay.
         */
        public void ManualFlush()
        {
            telemetryClient.Flush();
        }
    }
}
