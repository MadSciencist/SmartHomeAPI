using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHome.Contracts.TasmotaMqtt.Domain
{
    internal class BacklogBuilder
    {
        public (string topic, string payload) GetBacklog(IDictionary<string, string> items)
        {
            if (items is null || items.Count == 0) throw new ArgumentNullException(nameof(items));

            if (items.Count == 1)
            {
                var first = items.First();
                return (first.Key, first.Value);
            }
            else
            {
                var sb = new StringBuilder();
                foreach (var (key, value) in items)
                {
                    sb.Append($"{key} {value};");
                }

                return ("backlog", sb.ToString());
            }
        }
    }
}