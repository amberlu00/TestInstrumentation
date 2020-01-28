using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.IO;
using Newtonsoft.Json;

namespace TestInstrumentation
{
    public class CustomLogger
    {
        private ConcurrentDictionary<string, string> KeyToLog { get; set; }
        private BaseLogger Logger { get; set; }

        public CustomLogger(string appType)
        {
            Logger = new BaseLogger(appType);
            KeyToLog = new ConcurrentDictionary<string, string>();
            SetDefaultKeys();
        }

        public void SetDefaultKeys()
        {
            if (File.Exists("keys.json"))
            {
                using (StreamReader sr = new StreamReader("keys.json"))
                {
                    KeyToLog = JsonConvert
                        .DeserializeObject<ConcurrentDictionary<string, string>>(sr.ReadLine());
                }
            }

            KeyToLog.TryAdd("Exit", "Exiting the application.");
            KeyToLog.TryAdd("Start", "Starting the application.");
        }

        /**
         * Additional function to add new logging strings when necessary.
         * @key: The keyword to map to the template
         * @value: the interpolated template
         */
        public void AddUniqueLogType(string key, string value)
        {
            KeyToLog.AddOrUpdate(key, value, (k, v) => v);
        }

        /**
         * Call this to generate a log from a template.
         * @param sev: The severity of the message
         * @param key: The keyword which maps to the template
         * @param args: A variable number of string arguments
         */
        public void FormattedLog(string sev, string key, params string[] args)
        {
            FormattableString str = FormattableStringFactory
                .Create(KeyToLog[key], args);
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

        public void CloseApplication()
        {
            using (StreamWriter sw = new StreamWriter("keys.json", false))
            {
                sw.Write(JsonConvert.SerializeObject(KeyToLog));
            }
        }
    }
}