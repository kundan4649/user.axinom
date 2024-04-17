using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.customer
{
    /// <summary>User creation using email address request parameters</summary>
    public class CreateCustomerEmailCommand : IRequest<Result<Customer>>
    {
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
        [JsonProperty("first_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        /// <summary>The first name of the user</summary>
        [JsonProperty("last_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        
        /// <summary>Request sender's IP address</summary>
        [JsonIgnore]
        public string IpAddress { get; set; }
    }

    /// <summary> Create customer with email command validator</summary>
    public class CreateCustomerEmailCommandValidator : AbstractValidator<CreateCustomerEmailCommand>
    {
        /// <inheritdoc />
        public CreateCustomerEmailCommandValidator(IOptions<UserErrors> errors)
        {
            RuleFor(m => m.Password).Must(s => !string.IsNullOrWhiteSpace(s) && s.Trim().Length >= 6)
                .WithMessage(errors.Value.InvalidPassword.Message);

            RuleFor(m => m.Email).Must(ValidateContactDetails.IsEmail).WithMessage(errors.Value.InvalidEmail.Message);
        }
    }
}