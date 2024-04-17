using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>Command to send random code to user that can be used for change forgotten password</summary>
    public class ForgotPasswordEmailCommand : ForgotPasswordCommand, IRequest<Result<Success>>
    {
        /// <summary>Type of the authentication method</summary>
        [JsonIgnore]
        public override AuthenticationMethod Type => AuthenticationMethod.Email;

        /// <summary>The email address of the user</summary>
        [JsonProperty("email", Required = Required.Always)]
        [Required]
        public string Email { get; set; }
    }

    /// <summary>Validator for sending password change code command</summary>
    public class ForgotPasswordEmailCommandValidator : AbstractValidator<ForgotPasswordEmailCommand>
    {
        /// <inheritdoc />
        public ForgotPasswordEmailCommandValidator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.Email).Must(ValidateContactDetails.IsEmail).WithMessage(errors.Value.InvalidEmail.Message);
        }
    } 
}