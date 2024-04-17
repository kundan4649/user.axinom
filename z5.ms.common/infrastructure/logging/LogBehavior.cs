using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;

namespace z5.ms.common.infrastructure.logging
{
    /// <summary>Logging behavior for mediator queries/commands pipeline</summary>
    public class LogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : IErrorResult
    {
        private readonly ILogger _logger;

        /// <inheritdoc />
        public LogBehavior(ILoggerFactory loggerFactory)
        {
            //_logger = loggerFactory.CreateLogger(typeof(TRequest).Name);
            _logger = loggerFactory.CreateLogger("LogBehaviour");
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogDebug($"Processing {request.PrettyPrint()}");
            var response = await next();
            _logger.LogDebug($"Processed {request.PrettyPrint()}");
            return response;
        }
    }
}