using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace z5.ms.common.extensions
{
    /// <summary>
    /// Extensions to enum values
    /// </summary>
    public static class EnumExtensions
    {
        private static readonly ConcurrentDictionary<string, string> ValueToEnumMemberMappings = new ConcurrentDictionary<string, string>();
        private static readonly ConcurrentDictionary<string, object> EnumMemberToValueMappings = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// Get the enum value as defined in an EnumMember attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EnumMemberValue<T>(this T value) where T : struct, IConvertible
            => ValueToEnumMemberMappings.GetOrAdd($"{nameof(T)}:{value}", k => LookupEnumMemberByValue(typeof(T), value.ToString(CultureInfo.InvariantCulture)));
        
        /// <summary>
        /// Look up enum value by EnumMember attribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="enumMember"></param>
        /// <returns></returns>
        public static object LookupEnumMember(Type type, string enumMember) 
            => string.IsNullOrEmpty(enumMember) ? null : EnumMemberToValueMappings.GetOrAdd($"{type.Name}:{enumMember}", k => LookupValueByEnumMemberAttribute(type, enumMember));
        
        /// <summary>
        /// Look up value by EnumMember attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumMember"></param>
        /// <returns></returns>
        public static T? LookupEnumMember<T>(string enumMember) where T : struct, IConvertible 
            => string.IsNullOrEmpty(enumMember) ? null : EnumMemberToValueMappings.GetOrAdd($"{nameof(T)}:{enumMember}", k => LookupValueByEnumMemberAttribute(typeof(T), enumMember)) as T?;



        private static string LookupEnumMemberByValue(Type type, string value)
        {
            var memberInfo = type.GetTypeInfo().DeclaredMembers
                .SingleOrDefault(x => x.Name == value);

            return memberInfo?.GetCustomAttribute<EnumMemberAttribute>(false)?.Value
                   ?? memberInfo?.Name;
        }

        private static object LookupValueByEnumMemberAttribute(Type type, string enumMember)
        {
            // unwrap nullable
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);
            
            // get member info for matching enum member
            var memberInfo = type.GetTypeInfo().DeclaredMembers
                .SingleOrDefault(x => 
                    (x.GetCustomAttributes(typeof(EnumMemberAttribute)).FirstOrDefault() as EnumMemberAttribute)
                     ?.Value?.Equals(enumMember, StringComparison.InvariantCultureIgnoreCase) 
                     ?? false);

            // get enum value from member info
            return memberInfo == null ? null : Enum.Parse(type, memberInfo.Name);
        }
    }
}
