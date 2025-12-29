using E_wallet.Domain.Context;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Infrastrucure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationDbContext _context;
        public ProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<Profile> AddAsync(Profile profile)
        {
             _context.Profiles.AddAsync(profile);
             _context.SaveChangesAsync();
            return Task.FromResult(profile);
        }

        public async Task<IEnumerable<Profile>> GetAllAsync()
        {
            return await _context.Profiles.AsNoTracking().ToListAsync();
        }

        public Task<Profile?> GetByIdAsync(int id)
        {
            return _context.Profiles
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task UpdateAsync(Profile profile)
        {
            _context.Profiles.Update(profile);
            return _context.SaveChangesAsync();

        }

        public Task<Profile?> GetByUserIdAsync(int userId)
        {
            //getting the profile by user id including the user and their default wallet
            //If no profile is found, it returns null
            return _context.Profiles
                .Include(p => p.User)
                    .ThenInclude(u => u.Wallets.Where(w => w.IsDefaultWallet))
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public Task<Profile?> GetByPhoneAsync(string phone,CancellationToken ct)
        {
            return _context.Profiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Phone == phone);
        }


    }
}
