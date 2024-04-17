using System;
using System.Net;
using MaxMind.GeoIP2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace z5.ms.common.infrastructure.geoip
{
    /// <summary>
    /// Geo IP service used to query location information for an IP address
    /// </summary>
    public interface IGeoIpService : IDisposable
    {
        /// <summary>Get the ISO 3166-1 alpha country code for the specified IP address</summary>
        /// <param name="ipAddress">The IP address used to find the country</param>
        /// <returns>A <see cref="Country"/> that matches the IP address or <see cref="Country.Unknown"/> if no matches were found</returns>
        Country LookupCountry(string ipAddress);
    }

    /// <inheritdoc />
    public class GeoIpService : IGeoIpService
    {
        private readonly ILogger _logger;
        private readonly string _databasePath;
        private DatabaseReader _reader;

        /// <inheritdoc />
        public GeoIpService(IConfiguration config, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType().Name);
            _databasePath = config.GetSection("GeoIP2DatabasePath").Value;
        }

        /// <inheritdoc />
        public Country LookupCountry(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress) || !IPAddress.TryParse(ipAddress, out _))
                return Country.Unknown;
            
            try
            {
                if (!Reader.TryCity(ipAddress, out var cityResponse))
                {
                    _logger.LogError($"Invalid IP address: {ipAddress}");
                    return Country.Unknown;
                }
    
                return new Country
                {
                    CountryCode = cityResponse.Country?.IsoCode,
                    CountryName = cityResponse.Country?.Name,
                    State = cityResponse.MostSpecificSubdivision?.Name,
                    TimeZoneOffset = GetTimeZoneOffset(cityResponse.Location?.TimeZone)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error accessing GeoIP Database", ex);
                return Country.Unknown;
            }
        }

        private double? GetTimeZoneOffset(string timeZone)
        {
            return string.IsNullOrWhiteSpace(timeZone) ? 0
                : DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZone)?
                      .GetUtcOffset(new Instant()).Seconds / 3600.0;
        }

        /// <inheritdoc />
        public void Dispose() => _reader?.Dispose();

        private DatabaseReader Reader => _reader ?? (_reader = new DatabaseReader(_databasePath));
    }
}
