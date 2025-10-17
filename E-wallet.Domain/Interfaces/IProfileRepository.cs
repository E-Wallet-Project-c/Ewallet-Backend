using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Domain.Interfaces
{
    public  interface  IProfileRepository
    {
        Task AddAsync(Profile profile);
        Task <Profile?> GetByIdAsync(int id);
        Task UpdateAsync(Profile profile);
        Task<IEnumerable<Profile>> GetAllAsync();
        Task<Profile?> GetByUserIdAsync(int userId);


    }
}
