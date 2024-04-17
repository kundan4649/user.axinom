using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace z5.ms.common.infrastructure.logging
{
    /// <inheritdoc />
    public class ElasticIlogger : ILogger
    {
        private readonly IElasticStackLogClient _elasticLogClient;
        private readonly string _categoryName;
        private readonly ILogger _logger;

        /// <inheritdoc />
        public ElasticIlogger(ILogger logger, IElasticStackLogClient elasticLogClient, string categoryName = null)
        {
            _logger = logger;
            _elasticLogClient = elasticLogClient;
            _categoryName = categoryName;
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _logger?.Log(logLevel, eventId, state, exception, formatter);

            _elasticLogClient.Send(new ElasticStackLogEntry
            {
                Level = logLevel,
                Exception = exception,
                Message = state.ToString(),
                Logger = _categoryName ?? eventId.Name,
                Timestamp = DateTime.UtcNow
            });
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel) => _logger.IsEnabled(logLevel);

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) => _logger.BeginScope(state);
    }

    /// <summary>Extension methods for adding elastic stack support to an ILogger instance.</summary>
    public static class LoggerElasticExtensions
    {
        /// <summary>Adds elastic support to an existing ILogger instance.</summary>
        /// <param name="logger"></param>
        /// <param name="config"></param>
        /// <param name="token"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public static ILogger ToElasticLogger(this ILogger logger, ElasticStackLogClientConfiguration config, string categoryName = null, CancellationToken? token = null) 
            => new ElasticIlogger(logger, ElasticStackLogClient.GetInstance(config, logger, token), categoryName);
    }
}