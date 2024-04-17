using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using z5.ms.common.abstractions;

namespace z5.ms.common.validation
{
    //https://github.com/JeremySkinner/FluentValidation/wiki/a.-Index
    //https://github.com/jbogard/MediatR/wiki/Behaviors
    /// <summary>Validation behavior for mediator queries/commands pipeline</summary>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : IErrorResult
    {
        private readonly IValidator<TRequest> _validator;
        private readonly ILogger _logger;

        /// <inheritdoc />
        public ValidationBehavior(IValidator<TRequest> validator, ILoggerFactory loggerFactory)
        {
            _validator = validator;
            //_logger = loggerFactory.CreateLogger(typeof(TRequest).Name);
            _logger = loggerFactory.CreateLogger("ValidationBehavior");
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogDebug("Validating request");
            var failures = _validator.Validate(request).Errors.Where(error => error != null).ToList();
            if (failures.Any())
            {
                var values = string.Join(",", failures.Select(a => a.AttemptedValue));
                _logger.LogInformation($"Request invalid - Failed values : '{values}'");
                return failures.ToError<TResponse>();
            }

            _logger.LogDebug("Request valid");
            var response = await next();
            return response;
        }
    }

    /// <summary>Extension methods to convert FluentValidation failures to API response object</summary>
    public static class ValidationErrorExtensions
    {
        private static TResponse GetResultInstance<TResponse>()
            where TResponse : IErrorResult
        {
            var returnValueType = typeof(TResponse).GetGenericArguments().Single();
            var constructed = typeof(Result<>).MakeGenericType(returnValueType);
            return (TResponse)Activator.CreateInstance(constructed);
        }

        //TODO: adjust error model to include validation errors
        //TODO: adjust error field to have error codes in addition to error message
        /// <summary>Convert FluentValidation failures to API response object</summary>
        public static TResponse ToError<TResponse>(this IList<ValidationFailure> failures)
            where TResponse : IErrorResult
        {
            var result = GetResultInstance<TResponse>();
            result.Success = false;
            result.Error = new Error
            {
                Code = 3,
                Message = "Invalid input parameter",
                Fields = failures.Select(f => new ErrorField { Field = f.PropertyName, Message = f.ErrorMessage }).ToList()
            };
            result.StatusCode = 400;
            return result;
        }
    }
}