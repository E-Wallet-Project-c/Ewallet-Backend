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
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _context;

        public WalletRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async  Task<Wallet?> GetWalletByIdAsync(int walletId)
        {
            return await _context.Wallets
             .Include(w => w.Transactions)
             .FirstOrDefaultAsync(w => w.Id == walletId && w.IsActive == true && w.IsDeleted == false);
        }

        public async  Task<IEnumerable<Transaction>> GetWalletTransactionsAsync(int walletId)
        {
            return await _context.Transactions
                .Where(t => t.WalletId == walletId && t.IsActive == true)
                .ToListAsync();
        }
        public async Task <List<Wallet>> GetWalletsByUserId(int userId)
        {
            //getting all wallets by user id (Async programming)
            return await _context.Wallets.Where(W =>W.UserId==userId).ToListAsync();
        }
        public async Task<Wallet> CreateWallet(Wallet wallet)
        {
           await _context.Wallets.AddAsync(wallet);
           await _context.SaveChangesAsync();
            return wallet;
        }
    }
}
