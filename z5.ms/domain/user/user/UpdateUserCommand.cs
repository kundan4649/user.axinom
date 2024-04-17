using System;
using FluentValidation;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.extensions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>Command for updating user details</summary>
    public class UpdateUserCommand : UserCommandBase
    {
        /// <summary>The email address of the user</summary>
        [JsonProperty("email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        /// <summary>The mobile number of the user</summary>
        [JsonProperty("mobile", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Mobile { get; set; }

        /// <summary>The new first name of the user</summary>
        [JsonProperty("first_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        /// <summary>The new first name of the user</summary>
        [JsonProperty("last_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        /// <summary>Some mac address - e.g. the one from the device the user registered with.</summary>
        [JsonProperty("mac_address", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string MacAddress { get; set; }

        /// <summary>The date when the user was born</summary>
        [JsonProperty("birthday", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Birthday { get; set; }

        /// <summary>Gender</summary>
        [JsonProperty("gender", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Gender? Gender { get; set; }

        [JsonIgnore]
        /// <summary>Raw body</summary>
        public string RawRequest { get; set; }
    }

    /// <summary>Validator for update user details command</summary>
    public class UpdateUserCommandValidator : UserCommandBaseValidator<UpdateUserCommand>
    {
        /// <inheritdoc />
        public UpdateUserCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
            RuleFor(m => m.Email).Must(s => s.IsNullOrEmpty() || ValidateContactDetails.IsEmail(s))
                .WithMessage(errors.Value.InvalidEmail.Message);

            //RuleFor(m => m.Mobile).Must(s => s.IsNullOrEmpty() || ValidateContactDetails.IsPhoneNumber(s))
            //    .WithMessage(errors.Value.InvalidEmail.Message);
            RuleFor(m => m.Mobile).Must(s => s.IsNullOrEmpty() || ValidateContactDetails.IsMobileNumber(s))
               .WithMessage(errors.Value.InvalidEmail.Message);
            
        }
    }
}