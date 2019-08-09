using System;
using System.Collections.Generic;

namespace SmartHomeMock.SensorMock.Infrastructure
{
    public static class ResolvingFuncProvider
    {
        public static Func<TKey, Type> GetDictionaryResolvingFunc<TKey>(IDictionary<TKey, Type> dict)
        {
            return (key => dict.ContainsKey(key) ? dict[key] : throw new Exception($"Cannot resolve service for key [{key}]"));
        }

        public static Func<TKey, Type> GetDictionaryResolvingFunc<TKey>(IDictionary<TKey, Type> dict, string errorMessage)
        {
            return GetDictionaryResolvingFunc(dict, new Exception(errorMessage));
        }

        public static Func<TKey, Type> GetDictionaryResolvingFunc<TKey>(IDictionary<TKey, Type> dict, Exception ex)
        {
            return (key => dict.ContainsKey(key) ? dict[key] : throw ex);
        }
    }
}