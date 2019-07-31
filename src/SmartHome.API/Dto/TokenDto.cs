using System;

namespace SmartHome.API.Dto
{
    public class TokenDto
    {
        public string Schema{ get; set; }
        public string Token { get; set; }
        public DateTime Issued{ get; set; }
        public DateTime ValidTo { get; set; }
    }
}
