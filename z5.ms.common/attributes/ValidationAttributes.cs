using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using PhoneNumbers;

namespace z5.ms.common.attributes
{
    /// <summary> Email address validation attribute </summary>
    public class EmailAttribute : ValidationAttribute
    {
        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace((string) value))
                return true;
            try
            {
                var _ = new System.Net.Mail.MailAddress((string)value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary> Phone number validation attribute </summary>
    public class PhoneNumberAttribute : ValidationAttribute
    {
        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace((string)value))
                return true;
            #region OLD PHONE NUMBERS LOGIC
            //var phoneUtil = PhoneNumberUtil.GetInstance();
            //try
            //{
            //    var fullNumber = phoneUtil.Parse($"+{value}", "44");
            //    return phoneUtil.IsValidNumber(fullNumber);
            //}
            //catch
            //{
            //    return false;
            //}
            #endregion

            #region NEW MOBILE NUMBER LOGIC
            string number = string.Empty;
            if (value != null)
            {
                number = value.ToString();
            }
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
            #endregion
        }
    }

    /// <summary> IP address validation attribute </summary>
    public class IpAddressAttribute : ValidationAttribute
    {
        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            return string.IsNullOrWhiteSpace((string)value) || 
                System.Net.IPAddress.TryParse((string)value, out var _);
        }
    }

    /// <summary>Basic password validation attribute</summary>
    public class PasswordAttribute : ValidationAttribute
    {
        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            var strValue = value as string;
            return !string.IsNullOrWhiteSpace(strValue) && strValue.Length >= 6;
        }
    }

    /// <summary> Greater than validation attribute </summary>
    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly double _greaterThan;

        /// <inheritdoc />
        public GreaterThanAttribute(double gt)
            : base($"Value must be greater than {gt}")
        {
            _greaterThan = gt;
        }

        /// <inheritdoc />
        public override bool IsValid(object value)
            => (double)value > _greaterThan;
    }

    /// <summary> Less than validation attribute </summary>
    public class LessThanAttribute : ValidationAttribute
    {
        private readonly double _lessThan;

        /// <inheritdoc />
        public LessThanAttribute(double lt)
            : base($"Value must be less than {lt}")
        {
            _lessThan = lt;
        }

        /// <inheritdoc />
        public override bool IsValid(object value)
            => (double)value < _lessThan;

    }

    /// <summary> Enum range validation attribute </summary>
    public class EnumRangeAttribute : ValidationAttribute
    {
        /// <inheritdoc />
        public override bool IsValid(object value)
            => Enum.IsDefined(value.GetType(), value);
    }

    /// <summary> Not empty validation attribute </summary>
    public class NotEmptyAttribute : ValidationAttribute
    {
        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            if (value is string s)
                return !string.IsNullOrWhiteSpace(s);

            if (value is Guid g)
                return g != Guid.Empty;

            if (value is DateTime d)
                return d != DateTime.MinValue;

            return value != Activator.CreateInstance(value.GetType());
        }
    }
}
