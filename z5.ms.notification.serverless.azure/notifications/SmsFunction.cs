using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using z5.ms.common.notifications;

namespace z5.ms.user.serverless.azure.notifications
{
    /// <summary> Function handler to send sms messages to SMS server </summary>
    public static class SmsFunction
    {
        /// <summary> Handle notification sms message </summary>
        public static async Task Handle(NotificationMessage message, NotificationOptions config, ILogger log)
        {
            await SendSms(message.To, message.Body, config.SmsHost, log);
        }

        private static async Task SendSms(string mobile, string body, string smsHost, ILogger log)
        {
            using (var client = new HttpClient())
            {
                var url = $"{smsHost}?mobileno={mobile}&messagetext={HttpUtility.UrlEncode(body.Trim())}";
                var result = await client.GetAsync(url);

                var start = DateTime.UtcNow;
                var response = await result.Content.ReadAsStringAsync();
                response = string.IsNullOrWhiteSpace(response) ? "No response message from server" : response;
                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception($"SendSms to {mobile} FAILED with status code: {(int)result.StatusCode} message: {response}. Request: {url}");
                }
                log.LogDebug($"It took {(DateTime.UtcNow - start).TotalMilliseconds} milliseconds to send sms to {mobile}");

                if (response.IndexOf("success", StringComparison.OrdinalIgnoreCase) < 0 && !response.Contains("OK"))
                    throw new Exception($"SendSms to {mobile} FAILED with status code: {(int) result.StatusCode} message: {response}. Request: {url}");

                log.LogInformation($"Sms sent to {mobile}: \"{body.Trim()}\"");
            }
        }
    }
}
