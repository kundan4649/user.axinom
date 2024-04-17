using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace z5.ms.common.infrastructure.events.model
{
    /// <summary>
    /// An event emitted when a user entity is created, updated or deleted.
    /// </summary>
    public class UserEvent
    {
        /// <summary>Unique id of the user</summary>
        public Guid Id { get; set; }

        /// <summary>The email address of the user (if available)</summary>
        public string Email { get; set; }

        /// <summary>The mobile phone number of the user (if available)</summary>
        public string Mobile { get; set; }

        /// <summary>The first name of the user</summary>
        public string FirstName { get; set; }

        /// <summary>The last name of the user</summary>
        public string LastName { get; set; }
        
        /// <summary>User system</summary>
        public string System { get; set; }

        /// <summary>Country in �ISO 3166-1 alpha-2� format from where the user initially registered.</summary>
        public string RegistrationCountry { get; set; }

        /// <summary>Defines the operation that has been performed on the user </summary>
        public UserEventType Type { get; set; }

        /// <summary>Event creation time</summary>
        public DateTime EventDatetime { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Valid types for UserEvents
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserEventType
    {
        /// <summary>Entity has been created </summary>
        Create,

        /// <summary>Entity has been updated </summary>
        Update,

        /// <summary>Entity has been marked as deleted </summary>
        Delete
    }
}