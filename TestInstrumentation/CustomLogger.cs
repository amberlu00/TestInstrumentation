using System;
using System.Collections.Generic;

namespace Application
{
    public class CustomLogger
    {
        private Dictionary<string, string> keyToLog;

        public CustomLogger()
        {
            Dictionary<string, string> keyToLog = new Dictionary<string, string>();
        }

        /**
         * Additional function to add new logging strings when necessary.
         */
        public void addUniqueLogType(string key, string value)
		{
            keyToLog.Add(key, value);
		}

    }
}
