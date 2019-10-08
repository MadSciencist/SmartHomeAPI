using System;
using System.Collections.Generic;

namespace SmartHome.Core.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConformed { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int ActivatedById { get; set; }
        public DateTime ActivationDate { get; set; }
        public bool IsActive { get; set; }
        public ICollection<int> CreatedNodes { get; set; }
        public ICollection<int> CreatedControlStrategies { get; set; }
        public ICollection<int> EligibleNodes { get; set; }
        public ICollection<UiConfigurationDto> UiConfigurations { get; set; }
    }
}
