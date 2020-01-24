# TestInstrumentation for COMP 410 Group A Instrumentation Library #


## Main Components

### Log Easily

Our instrumentation provides some added functionality on top of logging so that the Producer/Consumer teams don't have to explicitly format their strings before handing them to our module. Essentially, just give us the information snippets and we make them more verbose through our library.

### Log Flexibly

This package is a wrapper around a Serilog + Application Insights pipeline. This allows us to make whatever changes we deem necessary without disrupting the Producer/Consumer systems that already rely on us for their logging. Just make the calls to our library based on the CustomLogger.cs file and we will handle the rest 😀

### Example Usage

#### Logging

You will need to initialize an instance of the CustomLogger class in your code.
`CustomLogger cl = new CustomLogger(appType);`
where appType is the name of the application (such as "producer" or "consumer"). This is used in naming the file that the system logs to.

The log file will be named {appType}{date}.txt.

All log entries will be formatted in the following way.
`"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"`
(Example: "2020-01-22 19:22:07.240 -06:00 [INF] Received: 5")
1. Timestamp is the time the message was created. If you are using timespans, etc. then you will need to provide your own time data.
2. Level refers to the severity of the log (more below).
3. The message is the logged message.
4. Any exceptions are written afterwards.

We have the capability to either log in a preset formatted manner using string interpolation or the ability to log raw strings. Please not that the string interpolation is not completely tested yet!

#### Formatted logging
Say the consumer wants to log when something was received in the format "Received X of Y data", where X is the size of the data and Y is the type of data received.

We use string interpolation in C# to generate log message templates that reduce the amount of repetitive code needed for logging messages. Firstly, we have a key-value map that matches keywords to the templates. This means that instead of having to write out "Received X of Y data" each time you want to log that the consumer received data in your code, you can use a keyword, and the instrumentation library will know what format you want the log file to be in.

Serilog will automatically format the level of severity and a timestamp of when the log was received for you and will automatically newline at the end of comments.

For example:

1. Call the method `AddToLog(String key, String val)` which takes the key, val pair. Basically, this would just be defining a shorthand notation and storing it into the dictionary so that subsequent logs are faster and easier.

   - We use string interpolation in C# to generate log message templates that can 

   - In this case we'd call the method with something like `addToLog("received", "[severity]: producer received [quantity] of data at [timestamp] | [status]");` and this should only be done one time.

2. The map within the logging system would now have an entry:
   `{"received": "[severity]: producer received [quantity] of data at [timestamp] | [status]"}`
   which would then be formatted.
3. Call the method `FormattedLog(String sev, String key, params String[] args)` which will take in the severity, the shorthard key to look up the macro in our dictionary, and a variable number of arguments.

   For our example case, this method call would look like: `formattedLog("INFORMATION", "received", ["50MB", 7:00PM CST, OK])` and the method would just format the string before logging it to our sink _Application Insights_

Now, for subsequent logs of "received" type, you can just use that key and save yourself the trouble.

#### Unformatted logging
Alternatively, there exists a method called `RawLog(String sev, String info)` which allows you to log whatever you want in a "hard coded" way, in case you only need to log something once or for whatever reason don't want to use the shortcut we provided.
