using System;

namespace Matty.Framework.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Compare string with invariant culture and ignore case.
        /// </summary>
        /// <param name="a">First string</param>
        /// <param name="b">Second string</param>
        /// <returns>True when strings are matching, false otherwise.</returns>
        public static bool CompareInvariant(this string a, string b)
        {
            return (string.Compare(a, b, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}
