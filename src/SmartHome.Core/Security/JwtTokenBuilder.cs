using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartHome.Core.Entities.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartHome.Core.Security
{
    public class JwtTokenBuilder : ITokenBuilder
    {
        private readonly IConfiguration _config;

        public JwtTokenBuilder(IConfiguration config)
        {
            _config = config;
        }

        public (string token, DateTime expring) Build(AppUser user, IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(double.Parse(_config["Jwt:ValidTime"]));

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: signingCredentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}
