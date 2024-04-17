using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>Command for creating a new user with email activation</summary>
    public class RegisterEmailUserCommand : RegisterUserCommand, IRequest<Result<Success>>
    {
        /// <inheritdoc />
        [JsonIgnore]
        public override AuthenticationMethod Type => AuthenticationMethod.Email;

        /// <summary>The email address of the user</summary>
        [JsonProperty("email", Required = Required.Always)]
        [Required]
        public string Email { get; set; }

        /// <summary>The password of the user.</summary>
        [JsonProperty("password", Required = Required.Always)]
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 6)]
        public string Password { get; set; }

        /// <summary>The first name of the user</summary>
        [JsonProperty("first_name", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string FirstName { get; set; }

        /// <summary>The first name of the user</summary>
        [JsonProperty("last_name", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string LastName { get; set; }
    }

    /// <summary>Validator for registering user with email activation</summary>
    public class RegisterEmailUserCommandValidator : AbstractValidator<RegisterEmailUserCommand>
    {
        /// <inheritdoc />
        public RegisterEmailUserCommandValidator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.Email).Must(ValidateContactDetails.IsEmail).WithMessage(errors.Value.InvalidEmail.Message);

            RuleFor(c => c.Password).Must(p => !string.IsNullOrWhiteSpace(p) && p.Trim().Length >= 6)
                .WithMessage(errors.Value.InvalidPassword.Message);
        }
    }
}