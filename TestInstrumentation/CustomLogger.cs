using System;
using System.Collections.Generic;
using System.Threading;

namespace TestInstrumentation
{
    public class CustomLogger
    {
        private Dictionary<string, FormattableString> KeyToLog { get; set; }
        private BaseLogger Logger { get; set; }
        private Timer Timer { get; set; }

        public CustomLogger(string appType)
        {
            Logger = new BaseLogger(appType);
            KeyToLog = new Dictionary<string, FormattableString>();
            Timer = new Timer((e) => ManualFlush(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(30));
        }

        /**
         * Additional function to add new logging strings when necessary.
         * @key: The keyword to map to the template
         * @value: the interpolated template
         */
        public void AddUniqueLogType(string key, FormattableString value)
        {
            KeyToLog.Add(key, value);
        }

        /**
         * Call this to generate a log from a template.
         * @param sev: The severity of the message
         * @param key: The keyword which maps to the template
         * @param args: A variable number of string arguments
         */
        public void FormattedLog(string sev, string key, params string[] args)
        {
            FormattableString str = KeyToLog.GetValueOrDefault(key);
            RawLog(sev, string.Format(str.Format, args));
        }
        /**
         * Log a string directly to the sink.
         * @param sev: The severity of the message
         * @param info: The string to log
         */
        public void RawLog(string sev, string info)
        {
            switch (sev)
            {
                case "INFO":
                    Logger.LogStatus(info);
                    break;
                case "ERROR":
                    Logger.LogError(info);
                    break;
                case "WARN":
                    Logger.LogWarning(info);
                    break;
                case "FATAL":
                    Logger.LogFatal(info);
                    break;
                case "DEBUG":
                    Logger.LogDebug(info);
                    break;
                default:
                    Logger.LogWarning(sev + " is not a valid severity level.");
                    Logger.LogStatus(info);
                    break;
            }
        }

        /**
        * Flush the log to Azure. Whenever waiting for Azure can't wait, such as application shutdown, call this
        * function.
        */
        public void ManualFlush()
        {
            Logger.ManualFlush();
        }

        /**
        * If there is a function that the application calls on close, use this
        * code.
        */
        public void CloseLogger()
        {
            Timer.Dispose();
            ManualFlush();
        }
    }
}