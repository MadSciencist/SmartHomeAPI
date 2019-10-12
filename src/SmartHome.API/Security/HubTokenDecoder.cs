using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace SmartHome.API.Security
{
    public class HubTokenDecoder
    {
        private readonly IConfiguration _configuration;

        public HubTokenDecoder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (bool isAuth, int? id) GetClaimValues(HubCallerContext context)
        {
            var rawToken = context.GetHttpContext().Request.HttpContext.Request.Query["access_token"].ToString();

            if (string.IsNullOrEmpty(rawToken)) return (false, null);

            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.FromMinutes(0),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.InboundClaimTypeMap.Clear(); // This is the trick to get rid of microsoft schemas...
            var decodedToken = tokenHandler.ValidateToken(rawToken, validationParams, out var token);

            var nameIdentifier = decodedToken?.Claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (int.TryParse(nameIdentifier, out var id))
            {
                return (true, id);
            }

            return (false, null);
        }
    }
}
