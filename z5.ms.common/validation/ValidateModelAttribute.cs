using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using z5.ms.common.abstractions;

namespace z5.ms.common.validation
{
    /// <summary>
    /// Validates the submitted viewmodel and returns an error message on failures
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.ModelState.IsValid) return;

            var errorFields = actionContext.ModelState
                .Where(e => e.Value.ValidationState == ModelValidationState.Invalid)
                .Select(e => new ErrorField {Field = e.Key, Message = e.Value.Errors.First().ErrorMessage})
                .ToList();

            var isEmptyError = errorFields.Count == 1 && string.IsNullOrWhiteSpace(errorFields.First().Message);

            actionContext.Result = new JsonErrorResult(new Error
            {
                Code = 3,
                Message = isEmptyError ? "Invalid Json provided" : "Invalid input parameter",
                Fields = isEmptyError ? null : errorFields,
            });
        }
    }
}