using System;

namespace z5.ms.common.extensions
{
    /// <summary>
    /// Extensions for object types
    /// </summary>
    public static class ObjectExtensions
    {        
        /// <summary>
        /// Extension to check if the object has a numeric type
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsNumericType(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Extension to get string value of the object for SQL queries
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SqlValue(this object o)
        {
            return o.IsNumericType() ? $"{o}" : $"'{o}'";
        }

        /// <summary>
        /// Get specified property value of a class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static T GetValue<T>(this object o, string property)
        {
            try { return (T)(o.GetType().GetProperty(property)?.GetValue(o) ?? default(T)); }
            catch { return default(T); }
        }
    }
}