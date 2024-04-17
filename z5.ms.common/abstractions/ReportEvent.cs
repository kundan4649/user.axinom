using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;

namespace z5.ms.common.abstractions
{
    /// <summary>Single reporting data event</summary>
    [JsonObject(MemberSerialization.OptIn, Title = "report")]
    public class ReportEvent
    {
        /// <summary>Level name from Log levels enumeration</summary>
        [JsonProperty("level", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Level { get; set; }

        /// <summary>Component name from Components enumeration</summary>
        [JsonProperty("component", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Component { get; set; }

        /// <summary>Area name from Areas enumeration</summary>
        [JsonProperty("area", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Area { get; set; }

        /// <summary>Client timestamp (when event happened in the component). Timestamp must be in ISO8601 format.</summary>
        [JsonProperty("timestamp", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Required]
        public string Timestamp { get; set; }

        /// <summary>Message string</summary>
        [JsonProperty("message", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Required]
        public string Message { get; set; }

        /// <summary>Error stack trace (debug only)</summary>
        [JsonProperty("stack_trace", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string StackTrace { get; set; }

        /// <summary>Device type. Required in IFE.</summary>
        [JsonProperty("device_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceType { get; set; }

        /// <summary>User agent value</summary>
        [JsonProperty("user_agent", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string UserAgent { get; set; }

        /// <summary>Event Type name e.g. "item_start"</summary>
        [JsonProperty("event_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string EventType { get; set; }

        /// <summary>Unique client Id</summary>
        /// <remarks>Required for IFE front end events. In OTT, it may be a unique ID of anonymous user or subscriber ID of logged-in user.</remarks>
        [JsonProperty("client_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ClientId { get; set; }

        /// <summary>Vehicle Id</summary>
        /// <remarks>Required for IFE front end events. Not relevent for OTT solutions.</remarks>
        [JsonProperty("vehicle_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string VehicleId { get; set; }

        /// <summary>Seat number.</summary>
        /// <remarks>Required for IFE front end events. Not relevent for OTT solutions.</remarks>
        [JsonProperty("seat_number", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string SeatNumber { get; set; }

        /// <summary> Asset Id.</summary>
        [JsonProperty("asset_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AssetId { get; set; }
    }

    /// <summary>Reporting events bag (envelope)</summary>
    /// <remarks>
    /// Contains multiple reporting events with the same Level, Component and Area.
    /// Events are serialized as JSON string (escaped)
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn, Title = "reports_bag")]
    public class EventsBag
    {
        /// <summary>Unique events bag identifier (auto-generated)</summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore, Required = Required.Always)]
        public Guid Id { get; set; }

        /// <summary>Bag generation timestamp (when bag was processed by the server)</summary>
        /// <remarks>ISO8601 Time string</remarks>
        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore, Required = Required.Always)]
        public string Timestamp { get; set; }

        /// <summary> Optional metadata</summary>
        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>JSON Array of reporting events serialized as string</summary>
        [JsonProperty("events", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ReportEvent> Events { get; set; }
    }

    /// <summary>A factory for generation of reporting event bags (envelops)</summary>
    public static class EventFactory
    {
        /// <summary>Create reporting event from specified event type, arguments, component and area</summary>
        /// <param name="eventType">Event Type name e.g. "item_start"</param>
        /// <param name="assetId">Asset id, e.g. "0-0-The Godfather" </param>
        /// <param name="component">Component name from Components enumeration</param>
        /// <param name="area">Area name from Areas enumeration (Default </param>
        public static ReportEvent CreateEvent(string eventType, string assetId, string component, string area)
            => new ReportEvent
            {
                Level = "info",
                Timestamp = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffff'Z'"),
                Component = component,
                Area = area,
                EventType = eventType,
                AssetId = assetId
            };

        /// <summary>Create error reporting event from error message</summary>
        /// <param name="error">Error message</param>
        /// <param name="component">Component name from Components enumeration</param>
        /// <param name="area">Area name from Areas enumeration (Default </param>
        public static ReportEvent CreateEvent(string error, string component, string area)
            => new ReportEvent
            {
                Message = error,
                Level = "error",
                Timestamp = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffff'Z'"),
                Component = component,
                Area = area
            };

        /// <summary>>Create error reporting event from Exception</summary>
        /// <remarks>Adds stack trace to reporting event</remarks>
        /// <param name="exception">System exception</param>
        /// <param name="component">Component name from Components enumeration</param>
        /// <param name="area">Area name from Areas enumeration (Default </param>
        public static ReportEvent CreateEvent(Exception exception, string component, string area)
            => new ReportEvent
            {
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Level = "error",
                Timestamp = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffff'Z'"),
                Component = component,
                Area = area
            };

        /// <summary>Create events bag from a single reporting event</summary>
        public static EventsBag CreateEventsBag(ReportEvent evt, IDictionary<string, string> metadata = null) => new EventsBag
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffff'Z'"),
                Metadata = metadata,
                Events = new List<ReportEvent> {evt}
            };

        /// <summary>Create multiple event bags from reporting events, grouping them by level, component and area</summary>
        /// <remarks>Only events with valid log levels are added to bags</remarks>
        [Obsolete("Use CreateEventsBags")]
        public static IEnumerable<EventsBag> CreateGroupedEventsBags(IList<ReportEvent> events, IDictionary<string, string> eventsBagMetadata = null)
            => CreateGroupedEventsBags(events, new EventValidator().IsValidLevel, eventsBagMetadata);

        /// <summary>Create multiple event bags from reporting events, grouping them by level, component and area</summary>
        /// <remarks>Only events with valid log levels are added to bags, defined by 'levelValidator' function</remarks>
        [Obsolete("Use CreateEventsBags")]
        public static IEnumerable<EventsBag> CreateGroupedEventsBags(IList<ReportEvent> events,
            Func<string, bool> levelValidator, IDictionary<string, string> eventsBagMetadata = null)
            => events
                .Where(e => levelValidator(e.Level))
                .GroupBy(e => new {e.Level, e.Area, e.Component})
                .Select(g => new EventsBag
                {
                    Id = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffff'Z'"),
                    Metadata = eventsBagMetadata,
                    Events = g.ToArray() // otherwise problems deserializing: Unable to find a constructor to use for type System.Linq.Grouping
                });

        /// <summary>
        /// Create an event bag from reporting events./>
        /// </summary>
        /// <param name="events"></param>
        /// <param name="eventsBagMetadata">Generic metadata</param>
        public static EventsBag CreateEventsBag(IList<ReportEvent> events, IDictionary<string, string> eventsBagMetadata = null) => new EventsBag
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffff'Z'"),
                Metadata = eventsBagMetadata,
                Events = events
            };
    }

    /// <summary>Reporting extensions</summary>
    public class EventValidator
    {
        /// <remarks>Convert log level name string to NLog.LogLevel enumeration</remarks>
        /// <param name="levelName">One of supported NLog.LogLevel enum values as string</param>
        /// <returns>True, if conversion is successful</returns>
        public bool IsValidLevel(string levelName)
            => !String.IsNullOrEmpty(levelName)
               && (levelName.Equals("Info", StringComparison.OrdinalIgnoreCase)
                   || levelName.Equals("Warn", StringComparison.OrdinalIgnoreCase)
                   || levelName.Equals("Error", StringComparison.OrdinalIgnoreCase)
                   || levelName.Equals("Fatal", StringComparison.OrdinalIgnoreCase));
    }
}