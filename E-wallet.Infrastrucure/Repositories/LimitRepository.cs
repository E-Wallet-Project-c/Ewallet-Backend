using E_wallet.Domain.Context;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Enums;
using E_wallet.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                await _context.Limits.AddAsync(limit);
                 await _context.SaveChangesAsync();
                Console.WriteLine(limit.Id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the limit.", ex);
            }
        }

        public async Task<Limit> GetLimitByScopeAsync(LimitScope scope)
        {
            try
            {
                var limit = await _context.Limits.FirstOrDefaultAsync(l => l.Scope == scope);
                return await Task.FromResult(limit);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the limit by scope.", ex);
            }
        }

        public async Task<Limit> GetLimitsByTypeAndScopeAsync(LimitType type, LimitScope scope)
        {

            try
            {
                var limit = await _context.Limits.FirstOrDefaultAsync(l => l.Type == type && l.Scope == scope);
                return await Task.FromResult(limit);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the limit by type.", ex);
            }
        }

        public async Task<Limit> GetLimitsByTypeAsync(LimitType type)
        {
            try
            {
                var limit = await _context.Limits.FirstOrDefaultAsync(l => l.Type == type);
                return await Task.FromResult(limit);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the limit by type.", ex);
            }
        }
    }
}
