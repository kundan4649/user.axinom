using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace z5.ms.domain.user.customer
{
    /// <summary>Command to set customer's password</summary>
    public class SetCustomerPasswordCommand : CustomerCommandBase
    {
        /// <summary>New password</summary>
        [FromQuery(Name = "password")]
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 6)]
        public string Password { get; set; }

        [JsonIgnore]
        public string ipaddress { get; set; }

        [JsonIgnore]
        public string Registrationcountry { get; set; }
    }

    /// <summary>Validator for changing password command</summary>
    public class SetCustomerPasswordCommandValidator : CustomerCommandValidator<SetCustomerPasswordCommand>
    {
        /// <inheritdoc />
        public SetCustomerPasswordCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
            RuleFor(m => m.Password).Must(s => !string.IsNullOrWhiteSpace(s) && s.Trim().Length >= 6)
                .WithMessage(errors.Value.InvalidPassword.Message);
        }
    }
}