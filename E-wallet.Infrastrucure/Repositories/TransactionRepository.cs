using E_wallet.Domain.Context;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Interfaces;
using EllipticCurve.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Infrastrucure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await  _context.SaveChangesAsync();
            return transaction;
        }

        public int GetAllByDay(int walletId, DateTime date)
        {
            var count = _context.Transactions
                .Where(t => t.WalletId == walletId 
                                                && t.CreatedAt.HasValue 
                                                && t.CreatedAt.Value.Date == date.Date)
                                                .Count();
            return count;
        }

        public decimal GetTotalAmountByDay(int walletId, DateTime date)
        {
            try
            {
                var totalAmount = _context.Transactions
                    .Where(t => t.WalletId == walletId
                                                    && t.CreatedAt.HasValue
                                                    && t.CreatedAt.Value.Date == date.Date)
                    .Sum(t => (decimal?)t.Amount) ?? 0;
                return totalAmount;
            }
            catch (Exception ex) 
            {
                throw new Exception("An error occurred while retrieving the limit by type.", ex);

            }
        }
    }
}
