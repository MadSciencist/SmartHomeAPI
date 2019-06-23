using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartHome.Core.Domain.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SmartHome.API.Security.Token
{
    public class JwtTokenBuilder : ITokenBuilder
    {
        private readonly IConfiguration _config;

        public JwtTokenBuilder(IConfiguration config)
        {
            _config = config;
        }

        public (string token, DateTime expring) Build(AppUser user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(double.Parse(_config["Jwt:ValidTime"]));

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: signingCredentials);

            // TODO PoC token.Payload["AuthNodes"] = new string[] { "asd1", "asd2", "fsdf3" };

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}
