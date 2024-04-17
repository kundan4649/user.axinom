using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>The login parameters</summary>
    public class LoginEmailUserCommand : LoginUserCommand, IRequest<Result<OAuthToken>>
    {
        /// <inheritdoc />
        [JsonIgnore]
        public override AuthenticationMethod Type => AuthenticationMethod.Email;

        /// <summary>The email address of the user.</summary>
        [JsonProperty("email", Required = Required.Always)]
        [Required]
        public string Email { get; set; }
        
        /// <summary>The password of the user.</summary>
        [JsonProperty("password", Required = Required.Always)]
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 6)]
        public string Password { get; set; }
    }

    /// <summary>Validator for logging in a user with email and password</summary>
    public class LoginEmailUserCommandValidator : AbstractValidator<LoginEmailUserCommand>
    {
        /// <inheritdoc />
        public LoginEmailUserCommandValidator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.Email).Must(ValidateContactDetails.IsEmail).WithMessage(errors.Value.InvalidEmail.Message);
            RuleFor(c => c.Password).Must(p => !string.IsNullOrWhiteSpace(p) && p.Trim().Length >= 6).WithMessage(errors.Value.InvalidPassword.Message);
        } 
    } 
}