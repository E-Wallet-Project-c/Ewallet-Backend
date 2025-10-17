using E_wallet.Application.Dtos.Request.Auth;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Mappers;
using E_wallet.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IConfiguration _configuration;
        //private readonly SessionMapper _mapper;
        public JwtService(ISessionRepository sessionRepository,
            IConfiguration configuration)
        {
            _sessionRepository = sessionRepository;
            _configuration = configuration;
        }
        #region Generate access and refresh token
        public async Task<AuthResponse> GenerateToken(GenerateTokenRequest request)
        {
            var accessToken   =  GenerateAccessToken(request);
            string refreshToken = await GenerateRefreshToken(request.UserId);
            
            return AuthMapper.ToResponse(accessToken.AccessToken,
                refreshToken,
                accessToken.Expiries);
        }
        #endregion


        #region Generate Refresh Token
        private async Task<string> GenerateRefreshToken(int userId)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");


            var refreshToken = GenerateRandomString();
            var refreshTokenExpiryInDays = DateTime.UtcNow.AddDays( int.Parse(jwtSettings["RefreshTokenExpirationInDays"]!));


            await SaveRefreshTokenToDatabase(userId, refreshToken, refreshTokenExpiryInDays);
            return refreshToken;
        }
        #endregion

        #region Save Refresh Token To DB
        private async Task SaveRefreshTokenToDatabase(int userId, string refreshToken, DateTime refreshTokenExpiryInDays)
        {
            var existingSession = await _sessionRepository.GetByUserIdAsync(userId);

            if (existingSession != null)
            {
                await _sessionRepository.DeleteAsync(existingSession.Id);
            }

            var newSession = SessionMapper.ToEntity(userId, refreshToken, refreshTokenExpiryInDays);
            await _sessionRepository.AddAsync(newSession);
        }
        #endregion

        #region Generate Random String
        private string GenerateRandomString()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
        #endregion

        #region Generate Access Token
        private  (string AccessToken, DateTime Expiries) GenerateAccessToken(GenerateTokenRequest request)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var issuer = jwtSettings["validIssuer"];
            var audience = jwtSettings["validAudience"];
            var expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["tokenExpirationInMinutes"]!));
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
            return  (tokenHandler.WriteToken(token), expires );
        }
        #endregion
    }
}
