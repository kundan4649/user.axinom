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
    /// <summary>Command to send confirmation to email address again</summary>
    public class ResendConfirmationEmailCommand : ResendConfirmationCommand, IRequest<Result<Success>>
    {
        /// <inheritdoc />
        [JsonIgnore]
        public override AuthenticationMethod Type => AuthenticationMethod.Email;

        /// <summary>The email address that was used before</summary>
        [JsonProperty("email", Required = Required.Always)]
        [FromBody]
        [Required] 
        public string Email { get; set; }
    }

    /// <summary>Validator for resending registration confirmation to email</summary>
    public class ResendConfirmationEmailCommandValidator : AbstractValidator<ResendConfirmationEmailCommand>
    {
        /// <inheritdoc />
        public ResendConfirmationEmailCommandValidator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.Email).Must(ValidateContactDetails.IsEmail).WithMessage(errors.Value.InvalidEmail.Message);
        }
    }
}