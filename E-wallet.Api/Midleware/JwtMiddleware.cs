using E_wallet.Application.Dtos.Request.Auth;
using E_wallet.Application.Interfaces;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace E_wallet.Api.Midleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
        {
            await _next(context);

            if (context.User.Identity?.IsAuthenticated == true)
            {
                StringValues authHeader = context.Request.Headers["Authorization"];
                if (authHeader == StringValues.Empty || !authHeader.ToString().StartsWith("Bearer "))
                {
                    return;
                }

                var token = authHeader.ToString().Substring("Bearer ".Length).Trim();
                var handler = new JwtSecurityTokenHandler();

                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);

                    var expirationTime = jwtToken.ValidTo;
                    var now = DateTime.UtcNow;
                    var timeRemaining = expirationTime - now;

                    var tokenLifetime = jwtToken.ValidTo - jwtToken.ValidFrom;

                    if (timeRemaining < (tokenLifetime / 2))
                    {
                        var claims = context.User.Claims;

                        GenerateTokenRequest claimsRequest = new GenerateTokenRequest
                        {
                            UserId = int.Parse(claims.First(c => c.Type == "UserId").Value),
                            FullName = claims.First(c => c.Type == "FullName").Value,
                            Email = claims.First(c => c.Type == "Email").Value,
                            Role = claims.First(c => c.Type == "Role").Value,
                        };

                        var (newAccessToken, newExpires) = await jwtService.GenerateAccessToken(claimsRequest);

                        // client need to check the header 
                        context.Response.Headers.Append("X-Set-Authorization", newAccessToken);
                    }
                }
            }
        }
    }
}