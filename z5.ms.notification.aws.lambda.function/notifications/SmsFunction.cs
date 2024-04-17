using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using z5.ms.common.notifications;

namespace z5.ms.notification.aws.lambda.function
{
    /// <summary> Function handler to send sms messages to SMS server </summary>
    public static class SmsFunction
    {
        /// <summary> Handle notification sms message </summary>       
        public static async Task Handle(NotificationMessage message, NotificationOptions config, ILogger log = null)
        {
            await SendSms(message.To, message.Body, config.SmsHost, log,config.B2BApiSecretToken);
        }
        private static async Task SendSms(string mobile, string body, string smsHost, ILogger log, string secretToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mobile))
                {
                    Console.WriteLine($"SendSMS to {mobile} FAILED with message: Invalid mobile number");
                    return;
                }
                mobile = mobile.Replace(" ", string.Empty);

                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(smsHost),
                        Content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, string>
                                  {
                                      {"mobileno", mobile},
                                      {"messagetext", body}
                                  }), Encoding.UTF8, "application/json"),

                    };

                    request.Headers.Add("accept", "text/html");
                    request.Headers.Add("SECRET-TOKEN", secretToken);

                    var result = await client.SendAsync(request);

                    var start = DateTime.UtcNow;
                    var response = await result.Content.ReadAsStringAsync();
                    response = string.IsNullOrWhiteSpace(response) ? "No response message from server" : response;
                    if (!result.IsSuccessStatusCode)
                    {
                        throw new Exception($"SendSms to {mobile} FAILED with status code: {(int)result.StatusCode} message: {response}. Request: {smsHost}");
                    }
                    Console.WriteLine($"It took {(DateTime.UtcNow - start).TotalMilliseconds} milliseconds to send sms to {mobile}");

                    if (response.IndexOf("success", StringComparison.OrdinalIgnoreCase) < 0 && !response.Contains("OK"))
                        throw new Exception($"SendSms to {mobile} FAILED with status code: {(int)result.StatusCode} message: {response}. Request: {smsHost}");

                    Console.WriteLine($"Sms sent to {mobile}: \"{body.Trim()}\"");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendSMS to {smsHost} FAILED with message: {ex.Message}");
            }
        }
    }
}
