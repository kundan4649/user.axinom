using System;
using System.Linq;
using z5.ms.common.extensions;

namespace z5.ms.common.assets.common
{
    /// <summary>
    /// Extension methods to check asset license validity. 
    /// </summary>
    public static class LicensingExtensions
    {
        /// <summary>Checks if access to an asset is allowed according to the license terms</summary>
        /// <param name="license">License to check</param>
        /// <param name="country">Country to check if it is in the license's allowed countries list</param>
        /// <param name="queryTime">Time to check if it is in the license's licensing timeframe</param>
        /// <returns></returns>
        public static bool IsAllowed(this Licensing license, string country, DateTime queryTime)
        {
            // TODO: remove obsolete check when no longer used
            if (license != null && license.LicensingPeriods.IsNullOrEmpty())
                return IsAllowedObsolete(license, country, queryTime);

            if (license == null || license.LicensingPeriods.IsNullOrEmpty())
                return true;

            if (country.IsNullOrEmpty())
                return false;

            var period = license.LicensingPeriods.FirstOrDefault(x => x.Key.Equals(country, StringComparison.InvariantCultureIgnoreCase)).Value;
            return period != null && IsValidTime(period, queryTime);
        }

        /// <summary>Checks if access to an asset is allowed according to the license terms</summary>
        /// <param name="license">License to check</param>
        /// <param name="country">Country to check if it is in the license's allowed countries list</param>
        /// <returns></returns>
        public static bool IsAllowed(this Licensing license, string country)
        {
            // TODO: remove obsolete check when no longer used
            if (license != null && license.LicensingPeriods.IsNullOrEmpty())
                return IsValidCountry(license, country);

            if (license == null || license.LicensingPeriods.IsNullOrEmpty())
                return true;

            return !country.IsNullOrEmpty() && license.LicensingPeriods.Any(x => x.Key.Equals(country, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>Checks if access to an asset is allowed according to the license terms</summary>
        /// <param name="license">License to check</param>
        /// <param name="queryTime">Time to check if it is in the license's licensing timeframe</param>
        /// <returns></returns>
        public static bool IsAllowed(this Licensing license, DateTime queryTime)
        {
            // TODO: remove obsolete check when no longer used
            if (license != null && license.LicensingPeriods.IsNullOrEmpty())
                return IsValidTime(license, queryTime);

            if (license == null || license.LicensingPeriods.IsNullOrEmpty())
                return true;

            return license.LicensingPeriods.Any(kv => IsValidTime(kv.Value, queryTime));
        }

        /// <summary>Returns the business type of asset according to specified country</summary>
        public static BusinessType? GetBusinessType(this EntityBase entity, string country)
        {
            if (country == null)
                return entity?.BusinessType ?? BusinessType.Premium;

            var periods = entity?.Licensing?.LicensingPeriods?.FirstOrDefault(x => x.Key.Equals(country, StringComparison.InvariantCultureIgnoreCase));
            return (periods?.Value?.BusinessType ?? entity?.BusinessType) ?? BusinessType.Premium;
        }

        /// <summary>Returns the business type of asset according to specified country</summary>
        public static BusinessType? GetBusinessTypeForEpisode(this EntityBase entity, string country)
        {
            if (country == null)
                return entity?.BusinessType;

            var periods = entity?.Licensing?.LicensingPeriods?.FirstOrDefault(x => x.Key.Equals(country, StringComparison.InvariantCultureIgnoreCase));
            return periods?.Value?.BusinessType ?? entity?.BusinessType;
        }

        /// <summary>Returns the tvod tier of asset according to specified country</summary>
        public static string GetTvodTier(this EntityBase entity, string country)
            => entity?.Licensing?.LicensingPeriods?.FirstOrDefault(x => x.Key.Equals(country, StringComparison.InvariantCultureIgnoreCase)).Value?.TvodTier;

        private static bool IsValidTime(LicensingPeriod period, DateTime queryTime)
            => queryTime > (period?.LicenseFrom ?? DateTime.MinValue) && queryTime <= (period?.LicenseUntil ?? DateTime.MaxValue);

        /// <summary>Checks if access to an asset is allowed according to the license terms</summary>
        [Obsolete("LicensingPeriods should be used")]
        public static bool IsAllowedObsolete(this Licensing license, string country, DateTime queryTime)
            => IsValidCountry(license, country) && IsValidTime(license, queryTime);

        [Obsolete("LicensingPeriods should be used")]
        private static bool IsValidCountry(Licensing license, string country)
            => license == null || license.LicenseCountries.IsNullOrEmpty() || license.LicenseCountries.Contains(country, StringComparer.OrdinalIgnoreCase);

        [Obsolete("LicensingPeriods should be used")]
        private static bool IsValidTime(Licensing license, DateTime queryTime)
            => queryTime > (license?.LicenseFrom ?? DateTime.MinValue) && queryTime <= (license?.LicenseUntil ?? DateTime.MaxValue);
    }
}