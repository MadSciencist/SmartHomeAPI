using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Core.Dto
{
    public class ControlStrategyDto
    {
        public int Id { get; set; }

        [Required, MinLength(1), MaxLength(250)]
        public string ControlStrategyName { get; set; }

        [Required, MaxLength(250)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int CreatedById { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Created { get; set; }

        public ControlStrategyDto()
        {
        }
    }
}
