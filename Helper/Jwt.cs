using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectWasel21.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProjectWasel21.Helper
{
    public class JwtHelper
    {
        private readonly IConfiguration _config;

        public JwtHelper(IConfiguration config)
        {
            _config = config;
        }

        // توليد Access Token
        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.Username ?? ""),
                new Claim(ClaimTypes.Role, user.Role ?? "")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "default_secret_key")
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // توليد Refresh Token
        public (string TokenRaw, string TokenHash, DateTime ExpiresAt) GenerateRefreshToken()
        {
            var tokenRaw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var expiresAt = DateTime.UtcNow.AddDays(7);

            // Store token as-is (not BCrypt hashed) so we can look it up by ==
            return (tokenRaw, tokenRaw, expiresAt);
        }
    }
}