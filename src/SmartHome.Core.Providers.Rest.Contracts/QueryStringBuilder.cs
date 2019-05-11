using System;
using System.Collections.Generic;

namespace SmartHome.Core.Control.Rest.Common
{
    public class QueryStringBuilder
    {
        private readonly UriBuilder _builder;

        public QueryStringBuilder(string baseUri)
        {
            _builder = new UriBuilder(baseUri);
        }

        public void Add(KeyValuePair<string, string> entity)
        {
            var query = $"{entity.Key}={entity.Value}";

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
