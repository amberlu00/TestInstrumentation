using System;
using Serilog;

namespace TestInstrumentation
{
    public class Logger
    {

        public Logger(string appType)
        {
            Log.Logger = new LoggerConfiguration().
                WriteTo.File(string.Concat(appType, ".txt"),
                rollingInterval: RollingInterval.Day).CreateLogger();
        }

        public void LogStatus(string info) {
            Log.Information(string.Concat("{Now}", info), DateTime.Now);
        }

        public void LogError(string info)
        {
            Log.Error(string.Concat("{Now}", info), DateTime.Now);
        }
    }
}
