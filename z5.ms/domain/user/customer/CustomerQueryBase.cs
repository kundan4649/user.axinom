using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.customer
{
    /// <summary>Command for basic customer queries using customer id. </summary>
    public class CustomerQueryBase : IRequest<Result<Customer>>
    {
        /// <summary>Customer Id</summary>
        [FromRoute(Name = "customer_id")]
        [JsonProperty("customerId", Required = Required.Always)]
        [Required]
        public Guid? CustomerId { get; set; }
    }
    
    /// <summary>Validator for basic customer queries </summary>
    public class CustomerQueryBaseValidator<TResult> : AbstractValidator<TResult> where TResult : CustomerQueryBase
    {
        /// <inheritdoc />
        public CustomerQueryBaseValidator(IOptions<UserErrors> errors)
        {
            RuleFor(m => m.CustomerId).Must(s => s.HasValue).WithMessage(errors.Value.MissingCustomerId.Message);
        }
    }
}