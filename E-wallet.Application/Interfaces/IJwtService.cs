using E_wallet.Application.Dtos.Request.Auth;
using E_wallet.Application.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Interfaces
{
    public interface IJwtService
    {
        Task<AuthResponse> GenerateToken(GenerateTokenRequest request);
        Task<(string AccessToken, DateTime Expires)> GenerateAccessToken(GenerateTokenRequest request);
        Task<string> GenerateRefreshToken(int userId);

    }
}
