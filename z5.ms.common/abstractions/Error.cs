using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using z5.ms.common.attributes;

namespace z5.ms.common.abstractions
{
    /// <summary>Error model</summary>
    /// <remarks>https://wiki.axinom.com/display/PA/Error+Handling</remarks>
    [JsonObject(MemberSerialization.OptIn, Title = "error")]
    public class Error
    {
        /// <summary>Unique integer error code</summary>
        [JsonProperty("code", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; set; } = 1;

        /// <summary>Error message in Neutral system language</summary>
        [JsonProperty("message", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        //TODO Rename or extend to better reflect purpose as optional additional error information
        /// <summary>If input fields were available for this API call, validation errors per field will be included.</summary>
        [JsonProperty("fields", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ErrorField> Fields { get; set; }

        /// <inheritdoc />
        public override string ToString()
            => $"[Error - code: {Code}, message: {Message}]";
    }

    /// <summary>Validation message for a specific field.</summary>
    public class ErrorField
    {
        /// <summary>The property name of the validated field.</summary>
        [JsonProperty("field", Required = Required.Always)]
        public string Field { get; set; }

        /// <summary>The value of the validated field.</summary>
        [JsonProperty("value", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        /// <summary>The validation message for the corresponding field.</summary>
        [JsonProperty("message", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }

    /// <summary>Error code extensions</summary>
    public static class ErrorExtensions
    {
        /// <summary>
        /// Get error model from one of the listed error codes in Error Codes enumerations.
        /// Usage: ErrorCodes.Success.GetError(), MenuErrorCodes.Unavailable.GetError().
        /// </summary>
        /// <param name="enumValue">A value from error codes enumeration</param>
        /// <param name="message">Optional message text</param>
        /// <returns>Error model with integer code and error message in neutral language</returns>
        public static Error GetError(this Enum enumValue, string message = null)
            => new Error
            {
                Code = Convert.ToInt32(enumValue),
                Message = String.IsNullOrEmpty(message)
                    ? enumValue.GetTitle()
                    : message
            };
        
        /// <summary>
        /// Set specified fields to be error's fields
        /// </summary>
        /// <param name="error">Error object that needs fields to be attached</param>
        /// <param name="errorFields">Fields that will be added to Error</param>
        /// <returns>Error with specified fields.</returns>
        public static Error WithFields(this Error error, params ErrorField[] errorFields)
        {
            error.Fields = new List<ErrorField>();
            error.Fields.AddRange(errorFields.ToList());
            return error;
        }
    }
}