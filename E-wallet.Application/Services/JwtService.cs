using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Services
{
    public class JwtService(IConfiguration configuration) : IJwtService
    {
        public TokenResponse GenerateToken(GenerateTokenRequest request)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            var issuer = jwtSettings["validIssuer"];
            var audience = jwtSettings["validAudience"];
            var expires = DateTime.UtcNow.AddMinutes(int.Parse( jwtSettings["tokenExpirationInMinutes"]!));
            var key = (jwtSettings["secretKey"]!);

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, request.UserId.ToString()),
                new (JwtRegisteredClaimNames.Email, request.Email),
                new (JwtRegisteredClaimNames.GivenName, request.FullName),
                new (ClaimTypes.Role, request.Role),

            };

            var descreptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha384Signature)

            };
            var tokenHandler = new JwtSecurityTokenHandler(); 
            var token = tokenHandler.CreateToken(descreptor);

            return new TokenResponse
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = tokenHandler.WriteToken(token),
                Expiries = expires
            };
        }
    }
}
