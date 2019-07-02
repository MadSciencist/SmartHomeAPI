using System;
using System.Collections.Generic;

namespace SmartHome.Core.RestClient
{
    public class QueryStringBuilder
    {
        private readonly UriBuilder _builder;

        public QueryStringBuilder(string baseUri)
        {
            _builder = new UriBuilder(baseUri);
        }

        public void Add(KeyValuePair<string, string> pair)
        {
            var query = $"{pair.Key}={pair.Value}";

            if (_builder.Query?.Length > 1)
            {
                _builder.Query = _builder.Query.Substring(1) + "&" + query;
            }
            else
            {
                _builder.Query = query;
            }
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
