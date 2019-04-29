using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Domain.Entity
{
    [Table("rest_template")]
    public class RestTemplate : EntityBase
    {
        [MaxLength(20)]
        public string Name { get; set; } // Single Relay
        [MaxLength(10)]
        public string HttpVerb { get; set; } // PUT
        [MaxLength(255)]
        public string UrlTemplate { get; set; } // /api/{device}/{deviceId}?apikey=123
        public ICollection<RestTemplateValues> TemplateValues { get; set; } // 1:

        public string BodyTemplate { get; set; } // { "{key}": "{value}" }
    }


    [Table("rest_template_value")]
    public class RestTemplateValues : EntityBase
    {
        // navigation properties
        public RestTemplate RestTemplate { get; set; }
        public int RestTemplateId { get; set; }

        [MaxLength(20)]
        public string Key { get; set; }
        [MaxLength(50)]
        public string Value { get; set; }
    }
}
