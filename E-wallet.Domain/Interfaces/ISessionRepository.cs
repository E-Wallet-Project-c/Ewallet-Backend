using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Domain.Interfaces
{
    public interface ISessionRepository
    {
        Task<Session?> GetByIdAsync(int id);

        Task AddAsync(Session session);
        Task<Session?> GetByUserIdAsync(int userId);
        Task<Session?> GetByRefreshTokenAsync(string refreshToken);

        Task UpdateAsync(Session session);
        Task DeleteAsync(int id);
    }
}
