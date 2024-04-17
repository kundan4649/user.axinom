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
using z5.ms.common.helpers;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.services
{
    /// <summary>
    /// Interface to get user information from different social media platforms
    /// </summary>
    public interface ISocialAuthService
    {
        /// <summary>
        /// Get user profile from Facebook
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<Result<UserEntity>> GetFacebookUser(UserEntity user, string accessToken);

        /// <summary>
        /// Get user profile from Google
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<Result<UserEntity>> GetGoogleUser(UserEntity user, string accessToken);

        /// <summary>
        /// Get user profile from Twitter 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<Result<UserEntity>> GetTwitterUser(UserEntity user, string accessToken);

        /// <summary>
        /// Get user profile from Amazon 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<Result<UserEntity>> GetAmazonUser(UserEntity user, string accessToken);

        /// <summary>
        /// Get a mock user profile for test purposes
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<Result<UserEntity>> GetDummyUser(UserEntity user, string accessToken);
    }

    /// <inheritdoc/>
    public class SocialAuthService : ISocialAuthService
    {
        private readonly UserServiceOptions _options;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor method of SocialAuthService
        /// </summary>
        /// <param name="options"></param>
        /// <param name="loggerFactory"></param>
        public SocialAuthService(IOptions<UserServiceOptions> options, ILoggerFactory loggerFactory)
        {
            _options = options.Value;
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        /// <inheritdoc />
        public async Task<Result<UserEntity>> GetFacebookUser(UserEntity user, string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return Result<UserEntity>.FromError(new Error
                {
                    Code = 3,
                    Message = "Facebook access token is invalid"
                });

            var url = $"https://graph.facebook.com/me?fields=id,first_name,last_name,email&access_token={accessToken}";

            var response = await HttpHelpers.HttpClient.GetAsync(url)
                .LogIfTimeout(TimeSpan.FromSeconds(1), _logger,
                    $"It took more than 1 sec to get response from Facebook for token '{accessToken}'");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    $"Response for token '{accessToken}' from Facebook :\n{await response.Content.ReadAsStringAsync()}");
                return Result<UserEntity>.FromError(new Error
                {
                    Code = 3,
                    Message = "Facebook access token is invalid"
                });
            }

            var profile = JsonConvert.DeserializeObject<FacebookProfile>(await response.Content.ReadAsStringAsync());
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.Email = profile.Email;
            user.FacebookUserId = profile.Id;

            return Result<UserEntity>.FromValue(user);
        }

        /// <inheritdoc />
        public async Task<Result<UserEntity>> GetGoogleUser(UserEntity user, string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return Result<UserEntity>.FromError(new Error
                {
                    Code = 3,
                    Message = "Google access token is invalid"
                });

            var url = _options.GoogleAuthApiUrl.Replace("{accessToken}", accessToken);

            var response = await HttpHelpers.HttpClient.GetAsync(url)
                .LogIfTimeout(TimeSpan.FromSeconds(1), _logger,
                    $"It took more than 1 sec to get response from Google for token '{accessToken}'");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    $"Response for token '{accessToken}' from Google :\n{await response.Content.ReadAsStringAsync()}");
                return Result<UserEntity>.FromError(new Error
                {
                    Code = 3,
                    Message = "Google access token is invalid"
                });
            }

            var profile = JsonConvert.DeserializeObject<GoogleProfile>(await response.Content.ReadAsStringAsync());
            user.FirstName = profile.Name.FirstName;
            user.LastName = profile.Name.LastName;
            user.Email = profile.Emails?.FirstOrDefault(a => a.Type == "account")?.Email;
            user.GoogleUserId = profile.Id;

            return Result<UserEntity>.FromValue(user);
        }

        /// <inheritdoc />
        public async Task<Result<UserEntity>> GetTwitterUser(UserEntity user, string accessToken)
        {
            var tokenParms = accessToken?.Split('|');
            if (tokenParms == null || tokenParms.Length != 2)
                return Result<UserEntity>.FromError(new Error
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

                    response = await HttpHelpers.HttpClient.SendAsync(request)
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

                    return Result<UserEntity>.FromError(new Error
                    {
                        Code = 3,
                        Message = err
                    });
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Response for token '{accessToken}' from Twitter :\n{await response.Content.ReadAsStringAsync()}");
                return Result<UserEntity>.FromError(new Error
                {
                    Code = 3,
                    Message = "Twitter access token is invalid"
                });
            }

            var profile = JsonConvert.DeserializeObject<TwitterProfile>(await response.Content.ReadAsStringAsync());
            user.FirstName = profile.Name.Split(' ').First();
            user.LastName = profile.Name.Split(' ').Last();
            user.Email = profile.Email;
            user.TwitterUserId = profile.Id;

            return Result<UserEntity>.FromValue(user);
        }

        /// <inheritdoc />
        public async Task<Result<UserEntity>> GetAmazonUser(UserEntity user, string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return Result<UserEntity>.FromError(new Error
                {
                    Code = 3,
                    Message = "Amazon access token is invalid"
                });

            var url = $"https://api.amazon.com/user/profile?access_token={accessToken}";

            var response = await HttpHelpers.HttpClient.GetAsync(url)
                .LogIfTimeout(TimeSpan.FromSeconds(1), _logger,
                    $"It took more than 1 sec to get response from Amazon for token '{accessToken}'");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    $"Response for token '{accessToken}' from Amazon : {await response.Content.ReadAsStringAsync()}");
                return Result<UserEntity>.FromError(new Error
                {
                    Code = 3,
                    Message = "Amazon access token is invalid"
                });
            }

            var profile = JsonConvert.DeserializeObject<AmazonProfile>(await response.Content.ReadAsStringAsync());
            user.FirstName = profile.Name.Split(' ').First();
            user.LastName = profile.Name.Split(' ').Last();
            user.Email = profile.Email;
            user.AmazonUserId = profile.UserId;

            return Result<UserEntity>.FromValue(user);
        }


        /// <inheritdoc />
        public async Task<Result<UserEntity>> GetDummyUser(UserEntity user, string accessToken)
        {
            await Task.CompletedTask;

            var userGuid = Guid.NewGuid();
            user.FirstName = userGuid.ToString().Split('-').FirstOrDefault();
            user.LastName = userGuid.ToString().Split('-').LastOrDefault();
            user.Email = $"{userGuid}@social_test.com";
            switch (accessToken)
            {
                case "facebook":
                    user.FacebookUserId = userGuid.ToString("N");
                    break;
                case "google":
                    user.GoogleUserId = userGuid.ToString("N");
                    break;
                case "twitter":
                    user.TwitterUserId = userGuid.ToString("N");
                    break;
                default:
                    return Result<UserEntity>.FromError(new Error
                    {
                        Code = 3,
                        Message = "Invalid access token"
                    });
            }

            return Result<UserEntity>.FromValue(user);
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
