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
        public Task AddAsync(Profile profile)
        {
             _context.Profiles.Add(profile);
            return _context.SaveChangesAsync();
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
    }
}
