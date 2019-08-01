using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SmartHome.Core.Domain.Entity;

namespace SmartHome.Core.Domain.User
{
    public class AppUser : IdentityUser<int>
    {
        public AppUser ActivatedBy { get; set; }
        public DateTime ActivationDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public ICollection<UiConfiguration> UiConfiguration { get; set; }
        public ICollection<Node> CreatedNodes { get; set; }
        public ICollection<ControlStrategy> CreatedControlStrategies { get; set; }
        public ICollection<AppUserNodeLink> EligibleNodes { get; set; }
    }
}
