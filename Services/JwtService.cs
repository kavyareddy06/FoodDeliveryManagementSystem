﻿using FoodDeliveryManagementSystem.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodDeliveryManagementSystem.Services
{
    public class JwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        public JwtService(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
        }
        public string GenerateToken(string username, string role, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
           {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId.ToString()),
        new Claim("role", role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
            var token = new JwtSecurityToken(
               issuer: _issuer,
               audience: _audience,
               claims: claims,
               expires: DateTime.Now.AddHours(1),
               signingCredentials: credentials
           );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
