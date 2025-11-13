using E_wallet.Domain.Context;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Enums;
using E_wallet.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks; 

namespace E_wallet.Infrastrucure.Repositories
{
    public class LimitRepository : ILimitRepository
    {
        private readonly ApplicationDbContext _context;

        public LimitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Limit limit)
        {
            await _context.Limits.AddAsync(limit);
        }

        public async Task<Limit?> GetLimitByScopeAsync(LimitScope scope)
        {
            return await _context.Limits.FirstOrDefaultAsync(l => l.Scope == scope);
        }

        public async Task<Limit?> GetLimitsByTypeAndScopeAsync(LimitType type, LimitScope scope)
        {
            return await _context.Limits.FirstOrDefaultAsync(l => l.Type == type && l.Scope == scope);
        }

        public async Task<Limit?> GetLimitsByTypeAsync(LimitType type)
        {
            return await _context.Limits.FirstOrDefaultAsync(l => l.Type == type);
        }

        public void Update(Limit limit)
        {
            _context.Limits.Update(limit);
        }

        public void Delete(Limit limit)
        {
            _context.Limits.Remove(limit);
        }
    }
}