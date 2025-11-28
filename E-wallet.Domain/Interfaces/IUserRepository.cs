using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Domain.Interfaces
{
    public interface IUserRepository
    {

        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
        Task<User> AddAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task UpadteChangesAsync(User user);

    }
}
