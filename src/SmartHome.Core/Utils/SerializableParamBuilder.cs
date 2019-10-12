using Newtonsoft.Json;
using System.Collections.Generic;

namespace SmartHome.Core.Utils
{
    public class SerializableParamBuilder
    {
        private readonly Dictionary<string, object> _params;

        public SerializableParamBuilder()
        {
            _params = new Dictionary<string, object>();
        }

        public SerializableParamBuilder Add<T>(string key, T value)
        {
            _params.Add(key, value);
            return this;
        }

        public string Build() => JsonConvert.SerializeObject(_params);

        public override string ToString() => Build();
    }
}
