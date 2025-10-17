using E_wallet.Domain.Context;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_wallet.Infrastrucure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly ApplicationDbContext _context;
        public SessionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Session session)
        {
            try 
            {
                await _context.Sessions.AddAsync(session);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
              throw new Exception("An error occurred while adding the session.", ex);
            }

            
            
        }

        public  async Task DeleteAsync(int sessionId)
        {
            var session = await _context.Sessions.FindAsync(sessionId);

            if (session != null)
            {
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Session?> GetByIdAsync(int id)
        {
            return await _context.Sessions.FindAsync(id);
        }

        public async Task<Session?> GetByRefreshTokenAsync(string refreshToken)
        {
            var now = DateTime.UtcNow;
            return await _context
                .Sessions
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                x.RefreshToken == refreshToken
                && x.IsActive == true);
        }

        public async Task<Session?> GetByUserIdAsync(int userId)
        {
            var session = await _context
                .Sessions
                .FirstOrDefaultAsync(x => x.UserId == userId);
            return session;

        }

        public async Task UpdateAsync(Session session)
        {
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
        }
    }
}
