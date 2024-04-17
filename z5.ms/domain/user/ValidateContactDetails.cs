using PhoneNumbers;
using System.Text.RegularExpressions;

namespace z5.ms.domain.user
{
    /// <summary>Validate emails and phone numbers</summary>
    // TODO Change to extensions
    public static class ValidateContactDetails
    {
        /// <summary>Validate an email address</summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
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
        public static bool IsPhoneNumber(string number)
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

        /// <summary>Validate a mobile number based on country code. 
        /// Indian mobile number 10 digit without country code. 
        /// Other than indian, 9 to 15 digits along with country code.</summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsMobileNumberforRegistration(string number)
        {
            try
            {
                if (!string.IsNullOrEmpty(number) && ((number.StartsWith("91") && number.Length == 12) || (number.StartsWith("+91") && number.Length == 13)))
                {
                    Regex regex = new Regex(@"^[6789]\d{9}$");
                    if (number.StartsWith("91") && number.Length == 12)
                    {
                        number = number.Substring(2);
                        if (regex.IsMatch(number))
                            return true;
                    }
                    else if (number.StartsWith("+91") && number.Length == 13)
                    {
                        number = number.Substring(3);
                        if (regex.IsMatch(number))
                            return true;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(number) && number.Length >= 9 && number.Length <= 15)
                        return Regex.IsMatch(number, @"^[1-9]\d*$");
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>Validate a mobile number based on country code. 
        /// Indian mobile number 10 digit without country code. 
        /// Other than indian, 9 to 15 digits along with country code.</summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsMobileNumber(string number)
        {
            try
            {
                if (!string.IsNullOrEmpty(number) && ((number.StartsWith("91") && number.Length == 12) || (number.StartsWith("+91") && number.Length == 13)))
                {
                    Regex regex = new Regex(@"^[6789]\d{9}$");
                    if (number.StartsWith("91") && number.Length == 12)
                    {
                        number = number.Substring(2);
                        if (regex.IsMatch(number))
                            return true;
                    }
                    else if (number.StartsWith("+91") && number.Length == 13)
                    {
                        number = number.Substring(3);
                        if (regex.IsMatch(number))
                            return true;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(number) && number.Length >= 9 && number.Length <= 15)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Validate an IP address</summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIpAddress(string ip)
        {
            return System.Net.IPAddress.TryParse(ip, out var _);
        }
    }
}
