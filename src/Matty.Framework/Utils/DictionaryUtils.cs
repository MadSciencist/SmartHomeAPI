using System.Collections.Generic;

namespace Matty.Framework.Utils
{
    public class DictionaryUtils
    {
        /// <summary>
        /// Gets the value from dictionary without throwing exception.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns>Value if key is present, null otherwise.</returns>
        public static TValue GetValue<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, out var value) ? value : default;
        }
    }
}
