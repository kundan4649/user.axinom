using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>Basic command for user operations that require user id</summary>
    public class ConfirmUserCommandv2 : IRequest<Result<Success>>
    {
        /// <summary>Type of the authentication method</summary>
        [JsonIgnore]
        public AuthenticationMethod Type { get; set; }

        /// <summary>The random confirmation code</summary>
        [JsonProperty("code", Required = Required.Always)]
        [FromBody]
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string Code { get; set; }

        /// <summary>The email or mobile of the user</summary>
        [JsonProperty("recipient_address", Required = Required.Always)]
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 6)]
        public string RecipientAddress { get; set; }
    }

    /// <summary>Validator for user commands that use user id</summary>
    public class ConfirmUserCommandv2Validator : AbstractValidator<ConfirmUserCommandv2>
    {
        /// <inheritdoc />
        public ConfirmUserCommandv2Validator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.Code).Must(s => !string.IsNullOrWhiteSpace(s)).WithMessage(errors.Value.ConfirmationMissing.Message);

            RuleFor(c => c.RecipientAddress).Must(s => !string.IsNullOrWhiteSpace(s)).WithMessage(errors.Value.UserNotFound.Message);
        }
    }
}