using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Notification.Server
{
    public static class Utilities
    {
        internal static string GenerateJwtToken(string p_username, JwtSecurityTokenHandler p_jwtTokenHandler, SymmetricSecurityKey p_securityKey)
        {
            if (string.IsNullOrEmpty(p_username))
            {
                throw new InvalidOperationException("Username is not specified.");
            }

            var claims = new[] { new Claim(ClaimTypes.Name, p_username) };
            var credentials = new SigningCredentials(p_securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken("ExampleServer", "ExampleClients", claims, expires: DateTime.Now.AddSeconds(60), signingCredentials: credentials);
            return p_jwtTokenHandler.WriteToken(token);
        }
    }
}