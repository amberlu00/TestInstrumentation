# TestInstrumentation for COMP 410 Group A Instrumentation Library #

Check Sharepoint for details! DM Amber or Myra with q's.

## Main Components

### Log Easily

Our instrumentation provides some added functionality on top of logging so that the Producer/Consumer teams don't have to explicitly format their strings before handing them to our module. Essentially, just give us the information snippets and we make them more verbose through our library.

### Log Flexibly

This package is a wrapper around a Serilog + Application Insights pipeline. This allows us to make whatever changes we deem necessary without disrupting the Producer/Consumer systems that already rely on us for their logging. Just make the calls to our library based on the CustomLogger.cs file and we will handle the rest 😀

### Example Usage

#### Producer

Say the producer wants to log:
`"INFORMATION: producer received 50MB of data at 7:00PM CST | STATUS: OK"`

What this breaks down to is:

1. Call the method `addToLog(String key, String val)` which takes the key, val pair. Basically, this would just be defining a shorthand notation and storing it into the dictionary so that subsequent logs are faster and easier.

   - In this case we'd call the method with something like `addToLog("received", "[severity]: producer received [quantity] of data at [timestamp] | [status]");` and this should only be done one time.

2. The map within the logging system would now have an entry:
   `{"received": "[severity]: producer received [quantity] of data at [timestamp] | [status]"}`
   which would then be formatted.
3. Call the method `formattedLog(String sev, String key, params String[] args)` which will take in the severity, the shorthard key to look up the macro in our dictionary, and a variable number of arguments.

   For our example case, this method call would look like: `formattedLog("INFORMATION", "received", ["50MB", 7:00PM CST, OK])` and the method would just format the string before logging it to our sink _Application Insights_

Now, for subsequent logs of "received" type, you can just use that key and save yourself the trouble.

Alternatively: there exists a method called `rawLog(String sev, String info)` which allows you to log whatever you want in a "hard coded" way, in case you only need to log something once or for whatever reason don't want to use the shortcut we provided.
