using Microsoft.Extensions.Logging;

namespace z5.ms.common.extensions
{
    /// <summary>A simple wrapper that returns an existing logger instance</summary>
    public class SimpleLoggerFactory : ILoggerFactory
    {
        private readonly ILogger _logger;

        /// <inheritdoc />
        public SimpleLoggerFactory(ILogger logger) => _logger = logger;

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName) => _logger;

        /// <inheritdoc />
        public void Dispose() { }

        /// <inheritdoc />
        public void AddProvider(ILoggerProvider provider) { }
    }

    /// <summary>Extensions to ILogger interface</summary>
    public static class LoggerExtensions
    {
        /// <summary>Create an ILoggerFactory wrapper around an ILogger</summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static ILoggerFactory ToFactory(this ILogger logger) => new SimpleLoggerFactory(logger);
    }
}
