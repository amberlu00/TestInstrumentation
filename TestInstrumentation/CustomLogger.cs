using System;
using System.Collections.Generic;
using TestInstrumentation;

namespace Application
{
    public class CustomLogger
    {
        private Dictionary<string, string> keyToLog;
        private BaseLogger logger;

        public CustomLogger(string appType)
        {
            logger = new BaseLogger(appType);
            keyToLog = new Dictionary<string, string>();
        }

        /**
         * Additional function to add new logging strings when necessary.
         */
        public void addUniqueLogType(string key, string value)
        {
            keyToLog.Add(key, value);
        }

        /**
         * Severity levels:
         */
        public void formattedLog(string sev, string key, params string[] args)
        {
            string str = String.Format(keyToLog.GetValueOrDefault(key), args);
            switch (sev)
            {
                case "INFO":
                    logger.LogStatus(str);
                    break;
                case "ERR":
                    logger.LogError(str);
                    break;
                case "WARN":
                    logger.LogWarning(str);
                    break;
                case "FATAL":
                    logger.LogFatal(str);
                    break;
                case "DEBUG":
                    logger.LogDebug(str);
                    break;
                default:
                    logger.LogWarning(sev + " is not a valid severity level.");
                    logger.LogStatus(str);
                    break;
            }
        }
        public void rawLog(string sev, string info)
        {
            switch (sev)
			{
                case "INFO":
                    logger.LogStatus(info);
                    break;
                case "ERROR":
                    logger.LogError(info);
                    break;
                case "WARN":
                    logger.LogWarning(info);
                    break;
                case "FATAL":
                    logger.LogFatal(info);
                    break;
                case "DEBUG":
                    logger.LogDebug(info);
                    break;
                default:
                    logger.LogWarning(sev + " is not a valid severity level.");
                    logger.LogStatus(info);
                    break;
            }
        }
    }
}