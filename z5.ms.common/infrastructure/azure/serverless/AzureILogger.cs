using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using z5.ms.common.infrastructure.logging;

namespace z5.ms.common.infrastructure.azure.serverless
{
    /// <summary>Maps a Microsoft.Azure.WebJobs.TraceWriter instance to the Microsoft.Extensions.Logging.ILogger interface.</summary>
    [Obsolete("Just use ILogger in function definition")]
    public class AzureILogger : ILogger
    {
        private readonly TraceWriter _traceWriter;
        private readonly string _categoryName;

        /// <inheritdoc />
        public AzureILogger(TraceWriter traceWriter, string categoryName = null)
        {
            _traceWriter = traceWriter;
            _categoryName = categoryName;
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) =>
            _traceWriter.Trace(new TraceEvent(ToTraceLevel(logLevel), FormatMsg(state.ToString()), _categoryName, exception));

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel) => _traceWriter.Level <= ToTraceLevel(logLevel);

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) => null;

        private string FormatMsg(string msg) => _categoryName != null ? $"{_categoryName}: {msg}" : msg;

        private static TraceLevel ToTraceLevel(LogLevel logLevel) =>
            logLevel == LogLevel.Trace ? TraceLevel.Verbose :
            logLevel == LogLevel.Debug ? TraceLevel.Verbose :
            logLevel == LogLevel.Information ? TraceLevel.Info :
            logLevel == LogLevel.Warning ? TraceLevel.Warning :
            logLevel == LogLevel.Error ? TraceLevel.Error :
            logLevel == LogLevel.Critical ? TraceLevel.Error :
            TraceLevel.Off;
    }

    /// <summary>Extension methods for mapping a Microsoft.Azure.WebJobs.TraceWriter instance to the Microsoft.Extensions.Logging.ILogger interface.</summary>
    public static class TraceWriterExtensions
    {
        /// <summary>Maps a Microsoft.Azure.WebJobs.TraceWriter instance to the Microsoft.Extensions.Logging.ILogger interface.</summary>
        /// <param name="traceWriter"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        [Obsolete("Just use ILogger in function definition")]
        public static ILogger ToILogger(this TraceWriter traceWriter, string categoryName = null) => new AzureILogger(traceWriter, categoryName);

        /// <summary>Create an ILoggerFactory wrapper around a tracewriter</summary>
        /// <param name="traceWriter"></param>
        [Obsolete("Just use ILogger in function definition")]
        public static ILoggerFactory ToILoggerFactory(this TraceWriter traceWriter) => new AzureILogger(traceWriter).ToFactory();

        /// <summary>Adds adds elastic stack support to an existing ILogger.</summary>
        /// <param name="logger"></param>
        /// <param name="token"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public static ILogger ToElasticLogger(this ILogger logger, string categoryName, CancellationToken token)
            => logger.ToElasticLogger(AzureHelpers.Configure<ElasticStackLogClientConfiguration>(), categoryName, token);
    }
}