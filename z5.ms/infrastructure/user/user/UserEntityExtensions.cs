using Microsoft.AspNetCore.Http;
using z5.ms.common.extensions;
using z5.ms.common.infrastructure.geoip;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.user
{
    /// <summary> Extensions for custom UserEntity operations </summary>
    public static class UserEntityExtensions
    {
        /// <summary>
        /// Set additional parameters in UserEntity
        /// </summary>
        /// <param name="user">The UserEntity the parameters are set for.</param>
        /// <param name="request">The relevant http request.</param>
        /// <param name="geoIpService">The service user country lookups are based on.</param>
        /// <returns></returns>
        public static UserEntity SetAdditionalParameters(this UserEntity user, HttpRequest request, IGeoIpService geoIpService)
        {
            // Get IP address if it's not provided as input parameter
            user.IpAddress = string.IsNullOrWhiteSpace(user.IpAddress) ? request.GetRemoteIp() : user.IpAddress;

            // Get country code and state using Country endpoint (maxmind) from subscription service if it's not provided
            if (string.IsNullOrWhiteSpace(user.RegistrationCountry))
                user.RegistrationCountry = geoIpService.LookupCountry(user.IpAddress).CountryCode;

            if (string.IsNullOrWhiteSpace(user.RegistrationRegion))
                user.RegistrationRegion = geoIpService.LookupCountry(user.IpAddress).State;

            var json = user.Json.ToJObject();
            // Set the original_user_agent using User-Agent header if it's not provided as input parameter
            if (string.IsNullOrWhiteSpace(json.GetValue("original_user_agent")?.ToString()))
                json.Add("original_user_agent", request.Headers["User-Agent"].ToString());
            user.Json = json.ToString();

            return user;
        }

        /// <summary>
        /// Set additional parameters in CustomerCreate
        /// </summary>
        /// <param name="user">The UserEntity the parameters are set for.</param>
        /// <param name="request">The relevant http request.</param>
        /// <param name="geoIpService">The service user country lookups are based on.</param>
        /// <returns></returns>
        public static CustomerCreate SetAdditionalParameters(this CustomerCreate user, HttpRequest request, IGeoIpService geoIpService)
        {
            // Get IP address if it's not provided as input parameter
            user.IpAddress = string.IsNullOrWhiteSpace(user.IpAddress) ? request.GetRemoteIp() : user.IpAddress;

            // Get country code and state using Country endpoint (maxmind) from subscription service if it's not provided
            if (string.IsNullOrWhiteSpace(user.RegistrationCountry))
                user.RegistrationCountry = geoIpService.LookupCountry(user.IpAddress).CountryCode;

            if (string.IsNullOrWhiteSpace(user.RegistrationRegion))
                user.RegistrationRegion = geoIpService.LookupCountry(user.IpAddress).State;

            var json = user.Additional;
            // Set the original_user_agent using User-Agent header if it's not provided as input parameter
            if (string.IsNullOrWhiteSpace(json.GetValue("original_user_agent")?.ToString()))
                json.Add("original_user_agent", request.Headers["User-Agent"].ToString());
            
            return user;
        }
    }
}
