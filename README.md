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

Read more about string interpolation [here](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated) and DM a member of the instrumentation team if you still need help. 

Serilog will automatically format the level of severity and a timestamp of when the log was received for you and will automatically newline at the end of comments.

Each time afterwards you would like to use this template, you merely have to supply the keyword as a parameter of the logging function in our library. You can use as many arguments as necessary, too, but they must be strings.

In addition, you will need to classify the severity of what you are logging. There are five levels. The quotes in parantheses are the parameter you should use to log at the level.

- Debug ("DEBUG")
- Information ("INFO")
- Warning ("WARN")
- Error ("ERROR")
- Fatal ("FATAL")

Please make sure that everyone on your team is using the same scale of severity when necessary! If you are not sure how to divide it up, [you can use the standards Serilog suggests](https://github.com/serilog/serilog/wiki/Configuration-Basics#minimum-level).

For example (going back to the "Received X of Y data example):

1. Call the method `AddToLog(String key, String val)` which takes the key, val pair. Basically, this would just be defining a shorthand notation and storing it into the dictionary so that subsequent logs are faster and easier.

In this case we'd call the method with something like `addToLog("received", "Received {quantity} of {type} data");` and this should only be done one time.

2. The map within the logging system would now have an entry:
   `{"received": "Received {quantity} of {type} data"}`
   which would then be formatted.
   
3. Call the method `FormattedLog(String sev, String key, params String[] args)` which will take in the severity, the shorthard key to look up the macro in our dictionary, and a variable number of arguments.

For our example case, this method call would look like: `FormattedLog("INFO", "received", "50 MB", "string")` and the method would just format the string before logging it to our sink.

`"2020-01-22 19:22:07.240 -06:00 [INF] Received 50 MB of string data"`

Now, for subsequent logs of "received" type, you can just use that key and save yourself the trouble.

#### Unformatted logging
Alternatively, there exists a method called `RawLog(String sev, String info)` which allows you to log whatever you want in a "hard coded" way, in case you only need to log something once or for whatever reason don't want to use the shortcut we provided.

#### How to use this library
We are currently figuring out how to export this library to a version control tool like Azure DevOps so that people can receive changes without having to redownload the .nupkg. For now, unfortunately there is no easy way to use the code. You will have to:
1. Pull the code from here.
2. In the command line of the repository you just pulled (TestInstrumentation), run `dotnet pack`.
3. In the Visual Studio instance of the code that wants to download the package, go to Project > Manage NuGet Packages > Configure Sources (in the same tab that is defaulted to nuget.org) and find the local folder that contains the .nupkg called "TestInstrumentation.1.0.0.nupkg". Set this folder as a new source.
4. Add the package.

Also, we have lots of testing to do. If (God bless u) you actually pull this code, let us know about bugs ASAP! <3

#### What's next
1. The code is largely untested.
2. Real DevOps ... soon
