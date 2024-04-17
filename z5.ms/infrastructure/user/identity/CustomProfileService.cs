using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using z5.ms.domain.user.datamodels;
using z5.ms.common.extensions;
using Newtonsoft.Json;

namespace z5.ms.infrastructure.user.identity
{
    /// <inheritdoc />
    /// <summary>Service to get custom profile info</summary>
    public class CustomProfileService : IProfileService
    {
        private readonly ITokenUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserServiceOptions _options;

        /// <inheritdoc />
        public CustomProfileService(ITokenUserRepository userRepository, ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory, IOptions<UserServiceOptions> options)
        {
            _userRepository = userRepository;
            _logger = loggerFactory.CreateLogger("IdentityService");
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
            // block start- validate the claims provided - works only for token renewal
            var deviceId = getDeviceIdFromClaims(context) ??"";
            //block end

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
                try
                {
                    var removesubscriptionsURLData = JsonConvert.DeserializeObject<List<System.Dynamic.ExpandoObject>>(subscriptions);
                    foreach (dynamic data in removesubscriptionsURLData)
                    {
                        if (data.additional != null)
                        {
                            ((IDictionary<string, object>)data.additional).Remove("feRedirectSuccessURL");
                        }
                        if (data.additional != null)
                        {
                            ((IDictionary<string, object>)data.additional).Remove("feRedirectFailURL");
                        }
                    }
                    subscriptions = JsonConvert.SerializeObject(removesubscriptionsURLData);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error Message '{ex.Message}' stack trace :\n{ex.StackTrace.ToString()}");
                }
                if (!string.IsNullOrWhiteSpace(subscriptions))
                    claims.Add(new Claim(Z5Claims.Subscriptions, subscriptions));

                var currCountry = context.Subject.Claims.FirstOrDefault(c => c.Type.Equals(Z5Claims.CurrentCountry));
                if (currCountry != null)
                    claims.Add(currCountry);
                //block start- below code only work for v3/user/logins because v1 and v2 endpoints will not contains deviceid as a parameter
                var json = user.Json.ToJObject();
                if (deviceId == "")
                {
                    deviceId = json.GetValue("deviceid")?.ToString();
                } 
                if (!string.IsNullOrWhiteSpace(deviceId))
                {
                    claims.Add(new Claim(Z5Claims.DeviceId, deviceId));
                }
                //block end
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

        public async Task<string> GetUserSubscriptions(string sub)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_options.SubscriptionApiUrl}/v1/manage/subscription/user/{sub}")
            };
            var token = _options.InternalApiSecret.Split('|')[0].Split(' ');
            request.SetToken(token[0], token[1]);
            try
            {
                var result = await client.SendAsync(request);
                if(result.IsSuccessStatusCode)
                    return await result.Content.ReadAsStringAsync();
                throw new Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message '{ex.Message}' stack trace :\n{ex.StackTrace.ToString()}");
                return null;
            }
        }
        public  string getDeviceIdFromClaims(ProfileDataRequestContext context)
        {
            string deviceId = "";
            
            try
            {
                var tempEntry = context.Subject.Identities ?? null;
                if (tempEntry != null)
                {
                    foreach (var a in tempEntry)
                    {
                        if (a.Claims != null)
                        {
                            var data = a.Claims.ToList<Claim>();
                            foreach (var b in data)
                            {
                                var currentClaimKey = b.Type;
                                if (currentClaimKey == Z5Claims.DeviceId)
                                    deviceId = b.Value;
                            }
                        }

                    }
                }
                return deviceId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message '{ex.Message}' stack trace :\n{ex.StackTrace.ToString()}");
                return "";
            }
        }
    }
}