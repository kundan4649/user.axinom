using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>Command for logging in a user with social media access token</summary>
    public class LoginSocialUserCommand : LoginUserCommand, IRequest<Result<OAuthToken>>
    {
        /// <summary>User access token acquired from social media source eg. Facebook, Google ..</summary>
        [JsonProperty("access_token", Required = Required.Always)]
        [Required] 
        public string AccessToken { get; set; }
    }

    /// <summary>Validator for logging in user with social media access token</summary>
    public class LoginSocialUserCommandValidator : AbstractValidator<LoginSocialUserCommand>
    {
        /// <inheritdoc />
        public LoginSocialUserCommandValidator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.AccessToken).Must(token => !string.IsNullOrWhiteSpace(token)).WithMessage(errors.Value.FacebookAccessTokenInvalid.Message);
        }
    }
}