using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Amazon.S3;
using Microsoft.Extensions.Options;

namespace z5.ms.common.notifications
{
    /// <summary>Country based options for notifications</summary>
    public class NotificationCountryOptions
    {
        /// <summary>The country these options relate to</summary>
        public string Country { get; set; }

        /// <summary>Emails will appear to be sent from this address. Note this will also be the reply-to address</summary>
        public string EmailFromAddress { get; set; }

        /// <summary>A support email address which can be included in  email templates using the placeholder [PLACEHOLDER_SupportEmail]</summary>
        public string SupportEmail { get; set; }
    }
    

    /// <summary>Provides country based options for email notifications</summary>
    public interface INotificationCountryOptionsProvider
    {
        /// <summary>Get options details by country</summary>
        /// <param name="country"></param>
        /// <returns></returns>
        NotificationCountryOptions OptionsForCountry(string country);
    }

    /// <summary>Standard implementation of country options provider loads country options from an xml file in notification templates dll</summary>
    public class NotificationCountryOptionsProvider : INotificationCountryOptionsProvider
    {
        private readonly ConcurrentDictionary<string, NotificationCountryOptions> _countryOptions;

        /// <summary>Loads values from CountryOptions.xml (optional). Base element NotificationCountryOptionsList should contain a list of NotificationCountryOptions elements</summary>
        /// <param name="options"></param>
        public NotificationCountryOptionsProvider(IOptions<NotificationOptions> options, IAmazonS3 amazonS3Client = null)
        {
            var l = amazonS3Client == null ?
                NotificationHelpers.GetXmlTemplate<List<NotificationCountryOptions>>(
                     options.Value.NotificationStorageConnection, "CountryOptions", "NotificationCountryOptionsList").Result
            : NotificationHelpers.GetXmlTemplateFromS3<List<NotificationCountryOptions>>(
                     amazonS3Client, options.Value.TemplateS3BucketName, "CountryOptions", "NotificationCountryOptionsList").Result;

            if (l == null)
                return;

            _countryOptions = new ConcurrentDictionary<string, NotificationCountryOptions>(
                l.Select(x => new KeyValuePair<string, NotificationCountryOptions>(x.Country, x))
                , StringComparer.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public NotificationCountryOptions OptionsForCountry(string country)
            => _countryOptions == null ? null 
                : (_countryOptions.TryGetValue(country.ToUpperInvariant(), out var sender) ? sender : null);
    }
}