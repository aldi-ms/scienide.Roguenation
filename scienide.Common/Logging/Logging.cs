namespace scienide.Common.Logging;

using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Concurrent;

public static class Logging
{
    private static readonly ConcurrentDictionary<string, ILogger> _namedLoggers = new();
    private static Logger _defaultLogger;

    static Logging()
    {
        _defaultLogger = new LoggerConfiguration()
            .WriteTo.File($"Logs\\roguenation-{DateTime.Today:yy-MM-dd}.log")
            .MinimumLevel.Debug()
            .CreateLogger();
    }

    /// <summary>
    /// Configures a named logger with a custom Serilog configuration.
    /// </summary>
    public static ILogger ConfigureNamedLogger(string name, LoggerConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Logger name cannot be null or whitespace.", nameof(name));

        var logger = configuration.CreateLogger();
        _namedLoggers[name] = logger;
        return logger;
    }

    /// <summary>
    /// Retrieves a named logger or falls back to the default logger.
    /// </summary>
    public static bool TryGetLogger(string name, out ILogger logger)
    {
        return _namedLoggers.TryGetValue(name, out logger!);
    }

    /// <summary>
    /// Closes and flushes all loggers.
    /// </summary>
    public static void CloseAndFlush()
    {
        foreach (var logger in _namedLoggers.Values)
            (logger as IDisposable)?.Dispose();

        _defaultLogger?.Dispose();
    }
}
