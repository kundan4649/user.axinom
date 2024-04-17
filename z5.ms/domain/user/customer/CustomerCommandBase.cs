using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;

namespace z5.ms.domain.user.customer
{
    /// <summary>Command for basic customer operations done using customer id. </summary>
    public class CustomerCommandBase : IRequest<Result<Success>>
    {
        /// <summary>Customer Id</summary>
        [FromRoute(Name = "customer_id")]
        [JsonProperty("customerId", Required = Required.Always)]
        [Required]
        public Guid? CustomerId { get; set; }
    }

    /// <summary>Validator for basic customer operations </summary>
    public class CustomerCommandValidator<TResult> : AbstractValidator<TResult> where TResult : CustomerCommandBase
    {
        /// <inheritdoc />
        public CustomerCommandValidator(IOptions<UserErrors> errors)
        {
            RuleFor(m => m.CustomerId).Must(s => s.HasValue).WithMessage(errors.Value.MissingCustomerId.Message);
        }
    }
}
