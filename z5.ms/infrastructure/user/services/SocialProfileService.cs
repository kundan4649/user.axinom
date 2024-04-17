using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TwitterOAuth.Base;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;
using System.Collections.Generic;
using z5.ms.infrastructure.user.identity;
using Microsoft.Extensions.Configuration;

namespace z5.ms.infrastructure.user.services
{
    /// <summary>Interface to get user information from different social media platforms</summary>
    public interface ISocialProfileService
    {
        /// <summary>
        /// Get user profile from the specified social media platform
        /// </summary>
        /// <param name="type">Type of the social media platform</param>
        /// <param name="accessToken">Access token which provided by the social media platform</param>
        /// <returns></returns>
        Task<Result<SocialProfile>> GetProfile(AuthenticationMethod type, string accessToken);
        Task<Result<List<SocialAppProfiles>>> GetProfiles(AuthenticationMethod type, string accessToken, string socialProfileId);
    }

    /// <inheritdoc/>
    public class SocialProfileService : ISocialProfileService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserServiceOptions _options;
        private readonly ILogger _logger;
        private readonly IAuthTokenService _tokenService;
        private readonly IConfiguration _config;
        /// <inheritdoc />
        public SocialProfileService(IOptions<UserServiceOptions> options, IHttpClientFactory httpClientFactory, ILogger<SocialProfileService> logger, IAuthTokenService tokenService, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = options.Value;
            _tokenService = tokenService;
            _config = config;
        }

        public Task<Result<SocialProfile>> GetProfile(AuthenticationMethod type, string accessToken)
        {
            switch (type)
            {
                case AuthenticationMethod.Facebook:
                    return GetFacebookUser(accessToken);
                case AuthenticationMethod.Google:
                    return GetGoogleUser(accessToken);
                case AuthenticationMethod.GoogleWithTokenId:
                    return GetGoogleUserByTokenId(accessToken);
                case AuthenticationMethod.Twitter:
                    return GetTwitterUser(accessToken);
                case AuthenticationMethod.Amazon:
                    return GetAmazonUser(accessToken);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public Task<Result<List<SocialAppProfiles>>> GetProfiles(AuthenticationMethod type, string accessToken, string socialProfileId)
        {
            switch (type)
            {
                case AuthenticationMethod.Facebook:
                    return GetFacebookUsers(accessToken, socialProfileId);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private async Task<Result<SocialProfile>> GetFacebookUser(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return Result<SocialProfile>.FromError(new Error
                {
                    Code = 3,
                    Message = "Facebook access token is invalid"
                });

            var url = $"https://graph.facebook.com/me?fields=id,first_name,last_name,email&access_token={accessToken}";

            var response = await _httpClientFactory.CreateClient().GetAsync(url)
                .LogIfTimeout(TimeSpan.FromSeconds(1), _logger, $"It took more than 1 sec to get response from Facebook for token '{accessToken}'");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Response for token '{accessToken}' from Facebook : {await response.Content.ReadAsStringAsync()}");
                return Result<SocialProfile>.FromError(new Error
                {
                    Code = 3,
                    Message = "Facebook access token is invalid"
                });
            }

            var profile = JsonConvert.DeserializeObject<FacebookProfile>(await response.Content.ReadAsStringAsync());
            return Result<SocialProfile>.FromValue(new SocialProfile
            {
                FirstName = profile.FirstName ?? "",
                LastName = profile.LastName ?? "",
                Email = profile.Email,
                Id = profile.Id
            });
        }

        async Task<Result<List<SocialAppProfiles>>> GetFacebookUsers(string accessToken, string socialProfileId)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return Result<List<SocialAppProfiles>>.FromError(new Error
                {
                    Code = 3,
                    Message = "Facebook access token is invalid"
                });

            var url = $"https://graph.facebook.com/v7.0/{socialProfileId}/ids_for_apps?access_token={accessToken}";

            var response = await _httpClientFactory.CreateClient().GetAsync(url)
                .LogIfTimeout(TimeSpan.FromSeconds(1), _logger, $"It took more than 1 sec to get response from Facebook for token '{accessToken}'");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Response for token '{accessToken}' from Facebook : {await response.Content.ReadAsStringAsync()}");
                return Result<List<SocialAppProfiles>>.FromError(new Error
                {
                    Code = 3,
                    Message = "Facebook access token is invalid"
                });
            }

            var profile = JsonConvert.DeserializeObject<FacebookAppIds>(await response.Content.ReadAsStringAsync());
            return Result<List<SocialAppProfiles>>.FromValue(profile.Data.Select(i => new SocialAppProfiles { AppId = i.App.Id, AppName = i.App.Name, SocialProfileId = i.Id }).ToList());
        }

        private async Task<Result<SocialProfile>> GetGoogleUser(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return Result<SocialProfile>.FromError(new Error
                {
                    Code = 3,
                    Message = "Google access token is invalid"
                });

            var url = _options.GoogleAuthApiUrl.Replace("{accessToken}", accessToken);

            var response = await _httpClientFactory.CreateClient().GetAsync(url)
                .LogIfTimeout(TimeSpan.FromSeconds(1), _logger,
                    $"It took more than 1 sec to get response from Google for token '{accessToken}'");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    $"Response for token '{accessToken}' from Google : {await response.Content.ReadAsStringAsync()}");
                return Result<SocialProfile>.FromError(new Error
                {
                    Code = 3,
                    Message = "Google access token is invalid"
                });
            }

            var profile = JsonConvert.DeserializeObject<GoogleProfile>(await response.Content.ReadAsStringAsync());
            return Result<SocialProfile>.FromValue(new SocialProfile
            {
                FirstName = profile.Name.FirstName ?? "",
                LastName = profile.Name.LastName ?? "",
                Email = profile.Emails?.FirstOrDefault(a => a.Type.ToLower() == "account")?.Email,
                Id = profile.Id
            });
        }

        private async Task<Result<SocialProfile>> GetGoogleUserByTokenId(string tokenId)
        {
            if (string.IsNullOrWhiteSpace(tokenId))
                return Result<SocialProfile>.FromError(new Error
                {
                    Code = 3,
                    Message = "Google token id is invalid"
                });

            var result = await _tokenService.ValidateGoogleJwtToken(tokenId);

            if (!result.IsValid)
            {
                _logger.LogError(
                    $"Response for token '{tokenId}' from Google : { (result.Error != null ? result.Error.Message : string.Empty)}");
                return Result<SocialProfile>.FromError(result.Error);
            }

            string googleClaimUrl = _config["GoogleClaimUrl"];

            return Result<SocialProfile>.FromValue(new SocialProfile
            {
                FirstName = result.ClaimsPrincipal.GetClaim($"{googleClaimUrl}givenname") ?? string.Empty,
                LastName = result.ClaimsPrincipal.GetClaim($"{googleClaimUrl}surname") ?? string.Empty,
                Email = result.ClaimsPrincipal.GetClaim($"{googleClaimUrl}emailaddress"),
                Id = result.ClaimsPrincipal.GetClaim($"{googleClaimUrl}nameidentifier")
            });
        }

        private async Task<Result<SocialProfile>> GetTwitterUser(string accessToken)
        {
            var tokenParms = accessToken?.Split('|');
            if (tokenParms == null || tokenParms.Length != 2)
                return Result<SocialProfile>.FromError(new Error
                {
                    Code = 3,
                    Message = "Twitter access token is invalid"
                });

            var url = "https://api.twitter.com/1.1/account/verify_credentials.json?include_email=true";

            var response = new HttpResponseMessage();
            var maxRetryCount = 2;
            for (var retry = 0; retry < maxRetryCount; retry++)
            {
                try
                {
                    var request = GetTwitterRequest(tokenParms, url);

                    response = await _httpClientFactory.CreateClient().SendAsync(request)
                        .LogIfTimeout(TimeSpan.FromSeconds(1), _logger, $"It took more than 1 sec to get response from Twitter for token '{accessToken}'")
                        .TimeoutAfter(TimeSpan.FromSeconds(retry * 10 + 1));
                    break;
                }
                catch (Exception e)
                {
                    if (e is TimeoutException || e.InnerException is TimeoutException && retry < maxRetryCount - 1)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5));
                        continue;
                    }
                    
                    var err = $"Twitter authentication failed : {e.Message}";
                    _logger.LogError(err);

                    return Result<SocialProfile>.FromError(new Error
                    {
                        Code = 3,
                        Message = err
                    });
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Response for token '{accessToken}' from Twitter : {await response.Content.ReadAsStringAsync()}");
                return Result<SocialProfile>.FromError(new Error
                {
                    Code = 3,
                    Message = "Twitter access token is invalid"
                });
            }

            var profile = JsonConvert.DeserializeObject<TwitterProfile>(await response.Content.ReadAsStringAsync());
            return Result<SocialProfile>.FromValue(new SocialProfile
            {
                FirstName = profile.Name?.Split(' ').FirstOrDefault() ?? "",
                LastName = profile.Name?.Split(' ').LastOrDefault() ?? "",
                Email = profile.Email,
                Id = profile.Id
            });
        }

        private async Task<Result<SocialProfile>> GetAmazonUser(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return Result<SocialProfile>.FromError(new Error
                {
                    Code = 3,
                    Message = "Amazon access token is invalid"
                });

            var url = $"https://api.amazon.com/user/profile?access_token={accessToken}";

            var response = await _httpClientFactory.CreateClient().GetAsync(url)
                .LogIfTimeout(TimeSpan.FromSeconds(1), _logger,
                    $"It took more than 1 sec to get response from Amazon for token '{accessToken}'");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    $"Response for token '{accessToken}' from Amazon : {await response.Content.ReadAsStringAsync()}");
                return Result<SocialProfile>.FromError(new Error
                {
                    Code = 3,
                    Message = "Amazon access token is invalid"
                });
            }

            var profile = JsonConvert.DeserializeObject<AmazonProfile>(await response.Content.ReadAsStringAsync());
            return Result<SocialProfile>.FromValue(new SocialProfile
            {
                FirstName = profile.Name?.Split(' ').FirstOrDefault() ?? "",
                LastName = profile.Name?.Split(' ').LastOrDefault() ?? "",
                Email = profile.Email,
                Id = profile.UserId
            });
        }

        private HttpRequestMessage GetTwitterRequest(string[] tokenParms, string url)
        {
            var consumerKey = _options.TwitterConsumerKey;
            var consumerSecret = _options.TwitterConsumerSecret;
            var token = tokenParms [0];
            var tokenSecret = tokenParms [1];

            var oauth = new OAuthBase();
            var nonce = oauth.GenerateNonce();
            var timestamp = oauth.GenerateTimeStamp();
            var signature = oauth.GenerateSignature(new Uri(url), consumerKey, consumerSecret, token, tokenSecret, "",
                "", "GET", timestamp, nonce, out var normalizedUrl, out var normalizedRequestParameters);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            request.Headers.Authorization =
                new AuthenticationHeaderValue("OAuth", $"oauth_consumer_key=\"{HttpUtility.UrlEncode(consumerKey)}\"," +
                                                       $"oauth_token=\"{HttpUtility.UrlEncode(token)}\"," +
                                                       $"oauth_signature_method=\"HMAC-SHA1\"," +
                                                       $"oauth_timestamp=\"{timestamp}\"," +
                                                       $"oauth_nonce=\"{nonce}\"," +
                                                       $"oauth_version=\"1.0\"," +
                                                       $"oauth_signature=\"{HttpUtility.UrlEncode(signature)}\"");
            return request;
        }
    }
}
