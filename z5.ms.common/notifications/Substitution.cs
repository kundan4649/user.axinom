using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.common.helpers;

namespace z5.ms.common.notifications
{
    /// <summary>Defines a text substitution in a notification template</summary>
    public class Substitution
    {
        /// <inheritdoc />
        public Substitution()
        { }

        /// <inheritdoc />
        public Substitution(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>Text to be replaced in the template</summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>Text to insert into the template</summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    /// <summary>Extension methods for notification templates</summary>
    public static class SubstitutionExtensions
    {
        /// <summary>Render a template's placeholders a list of substitutions</summary>
        /// <param name="template"></param>
        /// <param name="substitutions"></param>
        /// <returns></returns>
        public static string Render(this IEnumerable<Substitution> substitutions, string template) => substitutions
            .Aggregate(template, (current, s) => current.Replace(s.Key, s.Value));

        public static IEnumerable<Substitution> AddConfigSettingSubstitutions(this IEnumerable<Substitution> substitutions, NotificationOptions options, NotificationCountryOptions countryOptions) 
            => substitutions.ToList().Concat(new[]
            {
                new Substitution("[PLACEHOLDER_FrontEndUrl]", options.FrontEndUrl),
                new Substitution("[PLACEHOLDER_SupportEmail]", string.IsNullOrWhiteSpace(countryOptions?.SupportEmail) ? options.SupportEmail : countryOptions.SupportEmail)
            });
    }
    public static class PostNotification
    {
        /// <summary>
        /// Post the email queue data to PHP b2b api
        /// </summary>
        /// <returns>
        /// Returns an error in case the operation failed.
        /// </returns>
        public static async Task<Result<Success>> Postb2bNotificationData(Guid userid, Notification notification, string b2bApi)
        {
            try
            {
                var uri = $"{b2bApi}/email/resend-verification.php";
                var content = new NewNotification
                {
                    uid = userid,
                    emailid = notification?.To,
                    Type = notification?.Type.ToString(),
                    TemplateName = notification?.TemplateName,
                    Country = notification.CountryOptions?.Country,
                    Substitutions = notification?.Substitutions,
                };
                var body = JsonConvert.SerializeObject(content);
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(uri),
                    Method = HttpMethod.Post,
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };

                var response = await HttpHelpers.HttpClient.SendAsync(request);

                var responseContent = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode
                    ? Result<Success>.FromValue(new Success())
                    : Result<Success>.FromError(1, responseContent);
            }
            catch (Exception ex)
            {
                return Result<Success>.FromError(new Error { Code = 1, Message = "Could not process the request" });
            }

        }


        public class NewNotification
        {
            /// <summary>Type of notification: email or sms</summary>
            public string Type { get; set; }

            /// <summary>Destination email address / mobile number</summary>
            public string emailid { get; set; }

            /// <summary>Name of the template to use. Templates are loaded form resources</summary>
            public string TemplateName { get; set; }

            /// <summary>Notification sender details</summary>
            public string Country { get; set; }

            /// <summary>Replacement values for placeholders defined in the email template</summary>
            public IEnumerable<Substitution> Substitutions { get; set; }

            public Guid uid { get; set; }
        }
    }
}