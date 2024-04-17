using Microsoft.Extensions.Logging;

namespace z5.ms.common.infrastructure.logging
{
    /// <summary>A simple wrapper that returns an existing logger instance</summary>
    public class DummyLoggerFactory : ILoggerFactory
    {
        private readonly ILogger _logger;
        
        /// <inheritdoc />
        public DummyLoggerFactory(ILogger logger) => _logger = logger;

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName) => _logger;

        /// <inheritdoc />
        public void Dispose() { }

        /// <inheritdoc />
        public void AddProvider(ILoggerProvider provider) { }
    }

    /// <summary>Extension methods for mapping a Microsoft.Azure.WebJobs.TraceWriter instance to the Microsoft.Extensions.Logging.ILogger interface.</summary>
    public static class ILoggerFactoryExtensions
    {
        /// <summary>Create an ILoggerFactory wrapper around an existing ILogger instance</summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static ILoggerFactory ToFactory(this ILogger logger) => new DummyLoggerFactory(logger);
    }
}