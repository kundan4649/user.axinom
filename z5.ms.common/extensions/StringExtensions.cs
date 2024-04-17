using System;
using Newtonsoft.Json.Linq;
using PhoneNumbers;

namespace z5.ms.common.extensions
{
    /// <summary>Extension methods for strings</summary>
    public static class StringExtensions
    {
        /// <summary>True, if string is equal case insensitive</summary>
        public static bool EqualsIgnoreCase(this string str, string value)
            => str?.Equals(value, StringComparison.OrdinalIgnoreCase) ?? false;

        /// <summary>True, if string is equal case insensitive</summary>
        public static bool EqualsIgnoreCase(this object obj, string value) 
            => ((string)obj)?.Equals(value, StringComparison.OrdinalIgnoreCase) ?? false;

        /// <summary>True, if string contains case insensitive</summary>
        public static bool ContainsIgnoreCase(this string str, string value)
            => (str?.IndexOf(value, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0;

        /// <summary>True, if string is starts with case insensitive</summary>
        public static bool StartsWithIgnoreCase(this string str, string value)
            => str?.StartsWith(value, StringComparison.OrdinalIgnoreCase) ?? false;

        /// <summary>Get byte array out of the string</summary>
        public static byte[] GetBytes(this string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>Convert string to JObject </summary>

        public static JObject ToJObject(this string str)
        {
            try { return JObject.Parse(str); }
            catch { return new JObject(); }
        }

        /// <summary>Validate an email address</summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(this string email)
        {
            try
            {
                var _ = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Validate a phone number</summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(this string number)
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();
            try
            {
                var fullNumber = phoneUtil.Parse($"+{number}", "44");
                return phoneUtil.IsValidNumber(fullNumber);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Validate an IP address</summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIpAddress(this string ip)
        {
            return System.Net.IPAddress.TryParse(ip, out var _);
        }

        /// <summary>Add file extensions</summary>
        //[Obsolete("Remove it!!! It smells real bad!", false)]
        public static string AddFileExtension(this string s, string ext)
            => string.IsNullOrEmpty(s)
                ? null
                : s.EndsWith(ext)
                    ? s
                    : $"{s}{ext}";
        
        /// <summary>Convert string to Guid. Null if the string does not represent a valid GUID.</summary>
        public static Guid? ToGuid(this string str) => Guid.TryParse(str, out var parsedGuid) ? parsedGuid : (Guid?)null;
    }
}
