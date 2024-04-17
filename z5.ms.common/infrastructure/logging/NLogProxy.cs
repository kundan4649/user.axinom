using System;
using Microsoft.Extensions.Logging;
using NLog;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace z5.ms.common.infrastructure.logging
{
    /// <summary>
    /// Provides an NLogProxyLogger that proxies logs to NLog with filtered properties for elasticsearch
    /// </summary>
    public class NLogProxyProvider : ILoggerProvider
    {
        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName) => new NLogProxyLogger(categoryName);

        /// <inheritdoc />
        public void Dispose() { }

        /// <summary>
        /// A logger that proxies logs to NLog with filtered properties for elasticsearch
        /// </summary>
        public class NLogProxyLogger : ILogger
        {
            private readonly string _categoryName;
            private readonly NLog.ILogger _logger;

            /// <inheritdoc />
            public NLogProxyLogger(string categoryName)
            {
                _categoryName = categoryName;
                _logger = LogManager.GetLogger(_categoryName);
            }

            /// <inheritdoc />
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var logEvent = new LogEventInfo(ToNLogLevel(logLevel), _categoryName, state.ToString());

                // Custom exception properties for elasticsearch log target
                if (exception != null)
                {
                    logEvent.Properties["exception"] = new
                    {
                        stacktrace = exception.StackTrace,
                        message = exception.Message
                    };
                }

                _logger.Log(logEvent);
            }

            /// <inheritdoc />
            public bool IsEnabled(LogLevel logLevel) => true;

            /// <inheritdoc />
            public IDisposable BeginScope<TState>(TState state) => null;

            private static NLog.LogLevel ToNLogLevel(LogLevel logLevel) =>
                logLevel == LogLevel.Trace ? NLog.LogLevel.Trace :
                logLevel == LogLevel.Debug ? NLog.LogLevel.Debug :
                logLevel == LogLevel.Information ? NLog.LogLevel.Info :
                logLevel == LogLevel.Warning ? NLog.LogLevel.Warn :
                logLevel == LogLevel.Error ? NLog.LogLevel.Error :
                logLevel == LogLevel.Critical ? NLog.LogLevel.Fatal :
                NLog.LogLevel.Off;
        }
    }
}