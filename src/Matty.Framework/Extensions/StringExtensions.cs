using System;

namespace Matty.Framework.Extensions
{
    public static class StringExtensions
    {
        public static bool CompareInvariant(this string a, string b)
        {
            return (string.Compare(a, b, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}
