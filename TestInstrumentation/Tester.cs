using System;

namespace TestInstrumentation
{
    public class Tester
    {
        public static void Main(string[] args)
        {
            // Examples of general logging.
            Logger pLogger = new Logger("Producer");
            pLogger.LogError("Everything broke!!!!");
            Logger cLogger = new Logger("Consumer");
            cLogger.LogStatus("Everything is OK!!!");

            // Examples of specific logging.

        }
    }
}