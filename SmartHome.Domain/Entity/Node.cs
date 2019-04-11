using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartHome.Domain.User;

namespace SmartHome.Domain.Entity
{
    [Table("nodes")]
    public class Node
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Identifier { get; set; }

        [BindNever]
        public AppUser Creator { get; set; }

        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        public string NodeType { get; set; }
        public string RegistredProperties { get; set; }
        public string IpAddress { get; set; }
        public string GatewayAddress { get; set; }
        public bool IsOn { get; set; }
    }
}
