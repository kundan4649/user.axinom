using System;
using System.Linq;

namespace z5.ms.infrastructure.user.services
{
    /// <summary>Enumeration defining methods to generate a confirmation key</summary>
    public enum ConfirmationKeyType
    {
        /// <summary>A numeric confirmation key</summary>
        Numeric,

        /// <summary>An alphanumeric confirmation key</summary>
        Alphanumeric
    }

    // TODO: should this go?
    /// <summary>
    /// Password service to generate and verify password hashes    
    /// </summary>
    public static class ConfirmationCodeHelpers
    {
        private const string ConfirmationCodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const string NumericChars = "0123456789";

        /// <summary>Generate a random code</summary>
        /// <param name="type"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateConfirmationKey(ConfirmationKeyType type, int length) => 
            type == ConfirmationKeyType.Numeric ? GenerateNumericConfirmationKey(length) : GenerateConfirmationKey(length);
        
        /// <summary>Generates a confirmation key to activate a user account</summary>
        /// <param name="length">Length of the confirmation key</param>
        /// <returns></returns>
        public static string GenerateConfirmationKey(int length)
        {
            var random = new Random();
            return new string(Enumerable.Repeat(ConfirmationCodeChars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>Generates a numeric confirmation key to activate a user account</summary>
        /// <param name="length">Length of the confirmation key</param>
        /// <returns></returns>
        public static string GenerateNumericConfirmationKey(int length)
        {
            var random = new Random();
            return new string(Enumerable.Repeat(NumericChars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
