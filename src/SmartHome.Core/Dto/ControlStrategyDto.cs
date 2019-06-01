using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Core.Dto
{
    public class ControlStrategyDto
    {
        public int Id { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string ControlContext { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string ReceiveContext { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string ReceiveProvider { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string ControlProvider { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<CommandEntityDto> AllowedCommands { get; set; }

        //[BindNever]
        public int CreatedById { get; set; }
        //[BindNever]
        public DateTime Created { get; set; }
    }
}
