using Microsoft.AspNetCore.Identity;
using SmartHome.Domain.Entity;
using System;
using System.Collections.Generic;

namespace SmartHome.Domain.User
{
    public class AppUser : IdentityUser<int>
    {
        public AppUser ActivatedBy { get; set; }
        public DateTime ActivationDate { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Node> CreatedNodes { get; set; }
    }
}
