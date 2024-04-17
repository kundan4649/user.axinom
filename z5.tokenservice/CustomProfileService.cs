using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using z5.ms.domain.user.datamodels;

namespace z5.tokenservice
{
    /// <inheritdoc />
    /// <summary>Service to get custom profile info</summary>
    public class CustomProfileService : IProfileService
    {
        private readonly ITokenUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IdentityServerTools _identityServerTools;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserServiceOptions _options;

        /// <inheritdoc />
        public CustomProfileService(ITokenUserRepository userRepository, ILoggerFactory loggerFactory, IdentityServerTools identityServerTools, IHttpClientFactory httpClientFactory, IOptions<UserServiceOptions> options)
        {
            _userRepository = userRepository;
            _logger = loggerFactory.CreateLogger("IdentityService");
            _identityServerTools = identityServerTools;
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        /// <inheritdoc />
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
 
            _logger.LogDebug("Get profile called for subject {subject} from client {client} with claim types {claimTypes} via {caller}",
                context.Subject.GetSubjectId(),
                context.Client.ClientName ?? context.Client.ClientId,
                context.RequestedClaimTypes,
                context.Caller);
            
            var user = await _userRepository.FindBySubjectId(context.Subject.GetSubjectId());

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(Z5Claims.UserId, user.Id.ToString()),
                    new Claim(Z5Claims.System, user.System ?? ""),
                    new Claim(Z5Claims.ActivationDate, user.ActivationDate?.ToString("s", System.Globalization.CultureInfo.InvariantCulture) ?? ""),
                    new Claim(Z5Claims.CreatedDate, user.CreationDate?.ToString("s", System.Globalization.CultureInfo.InvariantCulture) ?? ""),
                    new Claim(Z5Claims.RegistrationCountry, user.RegistrationCountry ?? "")
                };

                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    claims.Add(user.IsEmailConfirmed
                        ? new Claim(Z5Claims.UserEmail, user.Email)
                        : new Claim(Z5Claims.UserEmailNotVerified, user.Email));
                }

                if (!string.IsNullOrWhiteSpace(user.Mobile))
                {
                    claims.Add(user.IsMobileConfirmed
                        ? new Claim(Z5Claims.UserMobile, user.Mobile)
                        : new Claim(Z5Claims.UserMobileNotVerified, user.Mobile));
                }

                // take the subscriptions and current country claims we added in DelegationGrantValidator
                var subscriptions = await GetUserSubscriptions(sub);
                if(!string.IsNullOrWhiteSpace(subscriptions))
                    claims.Add(new Claim(Z5Claims.Subscriptions, subscriptions));

                var currCountry = context.Subject.Claims.FirstOrDefault(c => c.Type.Equals(Z5Claims.CurrentCountry));
                if (currCountry != null)
                    claims.Add(currCountry);

                context.IssuedClaims = claims;
            }
        }

        /// <inheritdoc />
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userRepository.FindBySubjectId(sub);
            context.IsActive = user != null;
        }

        private async Task<string> GetUserSubscriptions(string sub)
        {
            // TODO: I don't have a good feeling about this, very smelly
            var token = await _identityServerTools.IssueJwtAsync(
                lifetime: 10,
                claims: new[]
                {
                    new Claim(JwtClaimTypes.Subject, sub),
                    new Claim(JwtClaimTypes.Scope, "subscriptionapi"),
                    new Claim(JwtClaimTypes.Audience, "subscriptionapi")
                });

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_options.SubscriptionApiUrl}/v1/subscription?translation=en&include_all=false")
            };
            request.SetBearerToken(token);
            try
            {
                var result = await client.SendAsync(request);
                if(result.IsSuccessStatusCode)
                    return await result.Content.ReadAsStringAsync();
                
                throw new Exception(result.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Fetching subscriptions failed. Error message: {ex}");
                return null;
            }
        }
    }
}