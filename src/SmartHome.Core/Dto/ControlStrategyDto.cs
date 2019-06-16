using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SmartHome.Core.Dto
{
    public class ControlStrategyDto
    {
        public int Id { get; set; }
        [Required, MinLength(1), MaxLength(50)]
        public string ControlContext { get; set; }
        [Required, MinLength(1), MaxLength(50)]
        public string ReceiveContext { get; set; }
        [Required, MinLength(1), MaxLength(50)]
        public string ReceiveProvider { get; set; }
        [Required, MinLength(1), MaxLength(50)]
        public string ControlProvider { get; set; }
        [Required, MaxLength(250)]
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public ICollection<ControlStrategyLinkageDto> Commands { get; set; }
        public ICollection<ControlStrategyLinkageDto> Sensors { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int CreatedById { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Created { get; set; }
    }
}
