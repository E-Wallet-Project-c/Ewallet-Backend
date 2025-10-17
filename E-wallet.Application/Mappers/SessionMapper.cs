using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Mappers
{
    public  static class SessionMapper
    {
        public static Session ToEntity(int userId, string refreshToken, DateTime refreshExpiresAt)
        {
            return new Session
            {
                UserId = userId,
                RefreshToken = refreshToken,
                RefreshExpiresAt = refreshExpiresAt,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };
        }


    }
}
