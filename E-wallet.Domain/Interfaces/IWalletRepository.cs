using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Domain.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetWalletByIdAsync(int walletId);
        Task<IEnumerable<Transaction>> GetWalletTransactionsAsync(int walletId);
    }
}
