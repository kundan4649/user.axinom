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
using System.Text;
using z5.ms.common.notifications;

namespace z5.ms.infrastructure.user.services
{
    public interface ISubscriptionAPIService
    {
        /// <summary>
        /// Get subscription plan details from launch API
        /// </summary>
        /// <param name="countryCode">User Country Code</param>
        /// <returns></returns>
        Task<string> GetSubscriptionPlanDetails(string countryCode);

        /// <summary>
        /// Create subscription for the given user
        /// </summary>
        /// <param name="planId">User unique identifier</param>
        /// <param name="ipAddresss">Ip address</param>
        /// <param name="countryCode">User country code</param>
        /// <returns></returns>
        Task<string> CreatePromotionalSubscription(string userId, string ipAddresss, string countryCode);
    }

    /// <inheritdoc/>
    public class SubscriptionAPIService : ISubscriptionAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserServiceOptions _options;
        private readonly ILogger _logger;
        private readonly IAuthTokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly IAWSSnsClient _snsClient;
        /// <inheritdoc />
        public SubscriptionAPIService(IOptions<UserServiceOptions> options, IHttpClientFactory httpClientFactory, ILogger<SubscriptionAPIService> logger, IAuthTokenService tokenService, IConfiguration config, IAWSSnsClient snsClient)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = options.Value;
            _tokenService = tokenService;
            _config = config;
            _snsClient = snsClient;
        }

        public async Task<string> GetSubscriptionPlanDetails(string countryCode)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_options.LaunchAPIUrl}/front/countrylist.php?ccode={countryCode}&version=2")
            };
        
            try
            {
                _logger.LogError($"Registration: launch API url '{request.RequestUri.AbsoluteUri}'");
                var result = await client.SendAsync(request);
                if (result.IsSuccessStatusCode)
                    return await result.Content.ReadAsStringAsync();
                throw new Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message '{ex.Message}' stack trace :\n{ex.StackTrace.ToString()}");
                return string.Empty;
            }
        }

        public async Task<string> CreatePromotionalSubscription(string userId,string ipAddresss,string countryCode)
        {
            string planId = string.Empty;
            Dictionary<string, string> requestParameter = new Dictionary<string, string>();
            try
            {
                dynamic results = JsonConvert.DeserializeObject(await GetSubscriptionPlanDetails(countryCode));
                object[] subscriptionPlanDetails = results?.ToObject<object[]>();
                dynamic firstOrDefaultPlanDetail = subscriptionPlanDetails?.FirstOrDefault();
                bool isIncentivePlanActive = firstOrDefaultPlanDetail != null ? firstOrDefaultPlanDetail.registration_incentive_plans.active.Value : false;

                if (!isIncentivePlanActive)
                {
                    string message = $"Registration: No active registration incentive plan found. UserId: {userId}";
                    _logger.LogError(message);
                    return message;
                }

                string[] planIds = new string[0];
                if (firstOrDefaultPlanDetail != null)
                    planIds = firstOrDefaultPlanDetail.registration_incentive_plans.promoted_packs.ToObject<string[]>();

                planId = planIds.FirstOrDefault();

                if (string.IsNullOrEmpty(planId))
                {
                    string message = $"Registration: Subscription plan not found. UserId: {userId}";
                    _logger.LogError(message);
                    return message;
                }

                var client = _httpClientFactory.CreateClient();
                requestParameter = new Dictionary<string, string>
                                  {
                                      {"customer_id", userId},
                                      {"subscription_plan_id", planId},
                                      {"subscription_end", DateTime.Now.AddDays(1).ToString()},
                                      {"ip_address", ipAddresss},
                                      {"country", countryCode},
                                      {"recurring_enabled", "false"},
                                      {"check_billingcycle", "true"}
                                  };

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_options.SubscriptionApiUrl}/v2/manage/subscription/"),


                    Content = new StringContent(JsonConvert.SerializeObject(requestParameter), Encoding.UTF8, "application/json")
                };
                _logger.LogError($"Registration: Request data for subscription API call {JsonConvert.SerializeObject(requestParameter)}.  UserId: {userId}");
                var token = _options.InternalApiSecret.Split('|')[0].Split(' ');
                request.SetToken(token[0], token[1]);
                request.Headers.Add("accept", "application/json");


                client.Timeout = TimeSpan.FromMilliseconds(_options.SubscriptionRequestTimeout);

                var result = await client.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    var responseData = await result.Content.ReadAsStringAsync();
                    _logger.LogError($"Registration: Response data for subscription API call {responseData}.  UserId: {userId}");
                    return responseData;
                }

                throw new Exception(result.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Registration: Error Message '{ex.Message}' stack trace :\n{ex.StackTrace.ToString()} UserId: {userId}");
                if (!string.IsNullOrEmpty(planId))
                {
                    await _snsClient.SendToSNS(requestParameter);
                    _logger.LogError($"Registration: Exception occured. Pushed request data to SNS for further processing. UserId: {userId}");
                }
                else
                {
                    string message = $"Registration: Subscription plan not found. UserId: {userId}";
                }
                return null;
            }
        }
    }
}
