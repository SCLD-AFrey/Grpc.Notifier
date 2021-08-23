﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using DidiSoft.OpenSsl;
using Microsoft.IdentityModel.Tokens;
using SteelCloud.Encryption;

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

        internal static X509Certificate2 GetServerCert(string p_filepath, string p_filename, SecureString p_password)
        {
            X509Certificate2 cert;

            if (!Directory.Exists(p_filepath))
            {
                Directory.CreateDirectory(p_filepath);
            }
            
            if (!File.Exists(Path.Combine(p_filepath, p_filename)))
            {
                var kp = EncryptionEngine.GenerateNewAsymmetricRsaKeyPair(KeyLength.Length1024);
                cert = EncryptionEngine.GenerateX509Certificate2FromRsaKeyPair(kp, "test cert");
                EncryptionEngine.SaveX509Certificate2ToFile(cert, Path.Combine(p_filepath, p_filename), p_password);
                Console.WriteLine($"CERT CREATED: {Path.Combine(p_filepath, p_filename)}");
            }
            else
            {
                cert = EncryptionEngine.LoadX509Certificate2FromFile(Path.Combine(p_filepath, p_filename), p_password);
                Console.WriteLine($"CERT LOADED: {Path.Combine(p_filepath, p_filename)}");
            }
            return cert;
        }
    }
}