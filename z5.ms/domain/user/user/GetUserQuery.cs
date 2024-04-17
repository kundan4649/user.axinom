using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>get user by id query</summary>
    public class GetUserQuery : IRequest<Result<User>>
    {
        /// <summary>User Id</summary>
        [JsonProperty("user_id", Required = Required.Always)]
        [Required]
        public Guid UserId { get; set; }
    }

    /// <summary>Validator for user query</summary>
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        /// <inheritdoc />
        public GetUserQueryValidator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.UserId).Must(id => id != Guid.Empty).WithMessage(errors.Value.MissingUserId.Message);
        }
    }
}