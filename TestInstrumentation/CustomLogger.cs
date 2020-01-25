using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.IO;
using System.Collections.Generic;
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
            foreach (String file in Directory.GetFiles("/log_config", "*.json"))
            {
                ReadFile(file);
            }
            

        }

        /**
         * Used for reading .json files to populate the dictionary on startup.
         * @param filename the name of the file
         */
        private async void ReadFile (string filename)
        {
            using StreamReader sr = new StreamReader(filename);
            // Read the stream to a string, and write the string to the console.
            string line = await sr.ReadToEndAsync();
            var temporary = JsonConvert.DeserializeObject<Dictionary<string, string>>(line);

            foreach(KeyValuePair<string, string> pair in temporary)
            {
                KeyToLog.TryAdd(pair.Key, pair.Value);
            }

        }

        /**
         * Additional function to add new logging strings when necessary.
         * This will permanently override a keyword!!!
         * @key: The keyword to map to the template
         * @value: the interpolated template
         */
        public void AddUniqueLogType(string key, string value)
        {
            KeyToLog.AddOrUpdate(key, value, (k, v) => v);
            using StreamWriter sr = new StreamWriter("/log_config/custom.json");
            sr.Write(",\n {0} : {1}", key, value);
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
                .Create(KeyToLog.GetValueOrDefault(key), args);
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
    }
}