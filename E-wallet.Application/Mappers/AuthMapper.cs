using E_wallet.Application.Dtos.Request.Auth;
using E_wallet.Application.Dtos.Response;
using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Mappers
{
    public class AuthMapper
    {
        public static AuthResponse ToResponse(string accessToken, string refreshToken, DateTime Expiries)
        {
            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiries = Expiries
            };
        }

        public static GenerateTokenRequest ToTokenRequest(User uesr)
        {
            return new GenerateTokenRequest
            {
                UserId = uesr.Id,
                FullName = uesr.FullName,
                Email = uesr.Email,
                Role = "User"
            };
        }
    }
}
