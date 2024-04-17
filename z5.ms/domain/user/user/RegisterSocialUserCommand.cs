using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>Command to register user with social media access token</summary>
    public class RegisterSocialUserCommand : RegisterUserCommand, IRequest<Result<OAuthToken>>
    {
        /// <summary>Access token</summary>
        [JsonProperty("token", Required = Required.Always)]
        [Required]
        public string AccessToken { get; set; }
    }

    /// <summary>Validator for registering user with social media access token</summary>
    public class RegisterSocialUserCommandValidator : AbstractValidator<RegisterSocialUserCommand>
    {
        /// <inheritdoc />
        public RegisterSocialUserCommandValidator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.AccessToken).Must(token => !string.IsNullOrWhiteSpace(token)).WithMessage(errors.Value.FacebookAccessTokenInvalid.Message);
        }
    }
}