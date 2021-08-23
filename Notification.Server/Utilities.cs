using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using DidiSoft.OpenSsl;
using Microsoft.IdentityModel.Tokens;
using SteelCloud.Encryption;
using Notification.Common;

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

            Collection<UserRepo.ClientUser> users = Common.UserRepo.Users();

            var user = users.Where(o => o.UserName == p_username.ToLower()).FirstOrDefault();

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var credentials = new SigningCredentials(p_securityKey, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                "ExampleServer", 
                "ExampleClients", 
                claims.ToArray(), 
                expires: DateTime.Now.AddSeconds(60), 
                signingCredentials: credentials);
            
            return p_jwtTokenHandler.WriteToken(token);
        }

        internal static X509Certificate2 GetServerCert(string p_filepath, string p_filename, SecureString p_password, out string p_filepathfull)
        {
            X509Certificate2 cert;
            p_filepathfull = Path.Combine(p_filepath, p_filename);

            if (!Directory.Exists(p_filepath))
            {
                Directory.CreateDirectory(p_filepath);
            }
            
            if (!File.Exists(p_filepathfull))
            {
                var kp = EncryptionEngine.GenerateNewAsymmetricRsaKeyPair(KeyLength.Length1024);
                cert = EncryptionEngine.GenerateX509Certificate2FromRsaKeyPair(kp, "test cert");
                EncryptionEngine.SaveX509Certificate2ToFile(cert, p_filepathfull, p_password);
            }
            else
            {
                cert = EncryptionEngine.LoadX509Certificate2FromFile(p_filepathfull, p_password);
            }
            return cert;
        }
    }
}