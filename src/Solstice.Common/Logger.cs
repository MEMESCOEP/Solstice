using System;
using System.Runtime.CompilerServices;

namespace Solstice.Common;

public static class Logger
{
    private static readonly object _lock = new object();
    
    private static List<LogMessage> _logMessages = new List<LogMessage>();
    
    /// <summary>
    /// Anything above this level will be printed to the console.
    /// </summary>
    public static LogLevel MinimumPrintLevel { get; set; } = LogLevel.Debug;
    
    public static void OverrideExceptionHandler()
    {
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            Log(LogLevel.Fatal, $"Unhandled exception occurred: \n {args.ExceptionObject.ToString() ?? "Unknown exception"}");
            DumpLogs();
        };
    }
    
    public static void Log(LogLevel level, string message, [CallerMemberName] string callsite = "",
        [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
    {
        if (level < MinimumPrintLevel) return;

        var logMessage = new LogMessage(level, message, callsite, filePath, lineNumber);
        lock (_lock)
        {
            Console.WriteLine(logMessage);
            _logMessages.Add(logMessage);
        }

        if (level == LogLevel.Fatal)
        {
            throw new Exception($"Fatal error logged: {message}");
        }
    }

    private static void DumpLogs()
    {
        // Write all current log messages to a file. In ./Logs/YYYY-MM-DD_HH-MM-SS.log
        var logFileName = $"Logs/{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log";
        Directory.CreateDirectory(Path.GetDirectoryName(logFileName) ?? throw new InvalidOperationException("Could not create log directory."));
        lock (_lock)
        {
            using (var writer = new StreamWriter(logFileName, append: true))
            {
                foreach (var logMessage in _logMessages)
                {
                    writer.WriteLine(logMessage);
                }
            }
        }
    }
}

public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error,
    Fatal // Throws exception after logging
}

public class LogMessage
{
    public LogLevel Level { get; }
    public string Message { get; }
    public string MemberName { get; }
    public string FilePath { get; }
    public int LineNumber { get; }
    public DateTime Timestamp { get; }

    public LogMessage(LogLevel level, string message, string memberName = "", 
                      string filePath = "", int lineNumber = 0)
    {
        Level = level;
        Message = message;
        MemberName = memberName;
        FilePath = filePath;
        LineNumber = lineNumber;
        Timestamp = DateTime.Now;
    }

    public override string ToString()
    {
        return $"{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}";
    }
}