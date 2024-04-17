namespace z5.ms.common.extensions
{
    /// <summary>
    /// Extensions for value types
    /// </summary>
    public static class StructExtensions
    {
        /// <summary>
        /// Return a nullable version of a value type. If the input value is default, return null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? DefaultToNull<T>(this T value) where T : struct
        {
            return value.Equals(default(T)) ? null : value as T?;
        }
    }
}
