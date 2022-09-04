using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace DynamicDataTable
{
    /// <summary>
    /// Value provider extension class.
    /// </summary>
    public static class ValueProviderExtensions
    {
        /// <summary>
        /// Get value or default from specified key from given value provider.
        /// </summary>
        /// <typeparam name="T">The primitive type.</typeparam>
        /// <param name="valueProvider">The value provider.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="defaultValue">The default value if not present.</param>
        /// <returns>The value associated with the key in <see cref="{T}"/>.</returns>
        public static T? GetValueOrDefault<T>(
            this IValueProvider valueProvider,
            string key,
            T? defaultValue = default)
        {
            if (valueProvider.TryGetValue(key, out var stringValue))
            {
                return (T)Convert.ChangeType(stringValue.ToString(), typeof(T));
            }

            return defaultValue;
        }

        /// <summary>
        /// Try to get value from specified key.
        /// </summary>
        /// <param name="valueProvider">The value provider.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="stringValues">The value to override</param>
        /// <returns>Return true if success, false otherwise.</returns>
        public static bool TryGetValue(
            this IValueProvider valueProvider,
            string key,
            out StringValues stringValues)
        {
            var value = valueProvider.GetValue(key);
            if (value.Length != 0)
            {
                stringValues = value.FirstValue;
                return true;
            }

            stringValues = default;
            return false;
        }
    }
}