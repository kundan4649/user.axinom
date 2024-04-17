using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;

namespace z5.ms.domain.user.user
{
    /// <summary>Basic command for user operations that require user id</summary>
    public class UserCommandBase : IRequest<Result<Success>>
    {
        /// <summary>User Id. Received from token. Shouldn't be accessible from API</summary>
        [JsonIgnore]
        public Guid UserId { get; set; }

        /// <summary>User Id from which the request is received. Shouldn't be accessible from API</summary>
        [JsonIgnore]
        public string IpAddress { get; set; }

        /// <summary>CountryCode from which the request is received. Shouldn't be accessible from API</summary>
        [JsonIgnore]
        public string CountryCode { get; set; }
    }

    /// <summary>Validator for user commands that use user id</summary>
    public class UserCommandBaseValidator<TCommand> : AbstractValidator<TCommand> where TCommand : UserCommandBase
    {
        /// <inheritdoc />
        public UserCommandBaseValidator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.UserId).Must(id => id != Guid.Empty).WithMessage(errors.Value.MissingUserId.Message);
        }        
    }
}