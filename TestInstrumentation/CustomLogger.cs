using System;
using System.Collections.Generic;
using TestInstrumentation;

namespace Application
{
    public class CustomLogger
    {
        private Dictionary<string, string> KeyToLog { get; set; }
        private BaseLogger Logger { get; set; }

        public CustomLogger(string appType)
        {
            Logger = new BaseLogger(appType);
            KeyToLog = new Dictionary<string, string>();
        }

        /**
         * Additional function to add new logging strings when necessary.
         */
        public void addUniqueLogType(string key, string value)
        {
            KeyToLog.Add(key, value);
        }

        /**
         * 
         */
        public void formattedLog(string sev, string key, params string[] args)
        {
            string str = String.Format(KeyToLog.GetValueOrDefault(key), args);
            switch (sev)
            {
                case "INFO":
                    Logger.LogStatus(str);
                    break;
                case "ERR":
                    Logger.LogError(str);
                    break;
                case "WARN":
                    Logger.LogWarning(str);
                    break;
                case "FATAL":
                    Logger.LogFatal(str);
                    break;
                case "DEBUG":
                    Logger.LogDebug(str);
                    break;
                default:
                    Logger.LogWarning(sev + " is not a valid severity level.");
                    Logger.LogStatus(str);
                    break;
            }
        }
        public void rawLog(string sev, string info)
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
    }
}