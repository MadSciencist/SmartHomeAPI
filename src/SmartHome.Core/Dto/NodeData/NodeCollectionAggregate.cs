using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SmartHome.Core.Dto.NodeData
{
    public class NodeCollectionAggregate
    {
        [JsonProperty("magnitudes")]
        public IDictionary<string, object> MagnitudeDictionary { get; private set; }

        [JsonProperty("timeStamps")]
        public ICollection<DateTime> Timestamps { get; private set; }

        [JsonProperty("metadata")]
        public IDictionary<string, MetadataDescriptor> MetadataDictionary { get; private set; }

        [JsonProperty("properties")]
        public ICollection<string> Properties { get; private set; }

        [JsonProperty("pagination")]
        public PaginationResult Pagination { get; set; }

        public NodeCollectionAggregate()
        {
            Timestamps = new List<DateTime>();
            Properties = new List<string>();
            MagnitudeDictionary = new Dictionary<string, object>();
            MetadataDictionary = new Dictionary<string, MetadataDescriptor>();
        }

        public void AddTimestamps(ICollection<DateTime> timestamps)
        {
            Timestamps = timestamps ?? throw new ArgumentNullException(nameof(ICollection<DateTime>));
        }

        public void AddProperty<T>(string propertyName, ICollection<T> values, MetadataDescriptor descriptor)
        {
            ValidateAndThrow(propertyName, values, descriptor);

            MagnitudeDictionary.Add(propertyName, values);
            MetadataDictionary.Add(propertyName, descriptor);
            Properties.Add(propertyName);
        }
        private void ValidateAndThrow<T>(string propertyName, ICollection<T> values, MetadataDescriptor descriptor)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));
            if (Timestamps == null) throw new InvalidOperationException("Add time stamps first");
            if (Timestamps.Count != values.Count)
                throw new InvalidOperationException(
                    "Collection length mismatch (timestamps length don't match values length");
        }
    }
}
