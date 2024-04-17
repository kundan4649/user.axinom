using System;
using System.Diagnostics;

namespace z5.ms.common.extensions
{
    /// <summary>DateTime conversion extensions</summary>
    public static class DateTimeExtensions
    {
        /// <summary>Jan 01 1970</summary>
        private static DateTime JanFirst1970 => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>Convert DateTime to Unix timestamp (seconds since Jan 01 1970)</summary>
        public static double ToUnixTimestamp(this DateTime dateTime)
            => Math.Ceiling((dateTime - JanFirst1970).TotalSeconds);

        /// <summary>Converts Unix timestamp to DateTime</summary>
        public static DateTime ToDateTime(this double unixTimestamp)
            => JanFirst1970.AddSeconds(unixTimestamp);

        /// <summary>Convert DateTime to Zulu date-time string</summary>
        /// <remarks>Used for reporting data formatting</remarks>
        public static string ToZuluDateTimeString(this DateTime dateTime)
            => dateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffff'Z'");

        /// <summary>
        /// Get a start time and end time for days around a set date.
        /// If startDay is null, DateTime.MinValue will be returned as startOfStartDay.
        /// If endDay is null, DateTime.MaxValue will be returned as endOfEndDay.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        public static (DateTime startOfStartDay, DateTime endOfEndDay) GetRange(this DateTime date, int? startDay, int? endDay)
        {
            var startOfStartDay = startDay == null ? DateTime.MinValue : date.AddDays((double)startDay);
            var endOfEndDay = endDay == null ? DateTime.MaxValue : date.AddDays((double)endDay).AddDays(1).AddMilliseconds(-1);
            return (startOfStartDay, endOfEndDay);
        }

        /// <summary>
        /// Check if date is between specified dates
        /// </summary>
        /// <remarks>
        /// Dates will be compared if they are not null otherwise will be returned as true 
        /// </remarks>
        /// <param name="date"></param>
        /// <param name="from"></param>
        /// <param name="until"></param>
        /// <returns></returns>
        public static bool IsDateTimeBetween(this DateTime date, DateTime? from, DateTime? until)
        {
            return (from == null || from < date) && (until == null || until > date);
        }

        /// <summary>
        /// Convert a datetime to Utc. If the datetime's kind is unspecified, then assume its Utc (not local time)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ToUtcUsingDefaultUtc(this DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Utc ? dateTime :
                dateTime.Kind == DateTimeKind.Unspecified ? TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZoneInfo.Utc) :
                TimeZoneInfo.ConvertTimeToUtc(dateTime);
        }

        /// <summary>
        /// Convert a datetime to utc 
        /// </summary>
        /// <param name="utcTicks"></param>
        /// <returns></returns>
        public static string ToZuluDateTimeString(long utcTicks)
        {
            return new DateTime(utcTicks, DateTimeKind.Utc).ToZuluDateTimeString();
        }
        
        /// <summary>
        /// Convert a datetime to utc 
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns></returns>
        public static long ToUtcTicks(string dateTimeString)
        {
            return DateTime.Parse(dateTimeString).ToUtcUsingDefaultUtc().Ticks;
        }

        /// <summary>
        /// Get elapsed time from a stopwatch
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="stop">Stop the stopwatch and give the last elapsed time</param>
        /// <returns></returns>
        public static TimeSpan Elapsed(this Stopwatch sw, bool stop = false)
        {
            if(stop)
                sw.Stop();
            return sw.Elapsed;
        }
    }
}