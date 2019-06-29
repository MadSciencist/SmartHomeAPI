using System;

namespace SmartHome.Core.Utils
{
    public static class StringExtensions
    {
        public static bool CompareInvariant(this string a, string b)
        {
            return (string.Compare(a, b, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}
