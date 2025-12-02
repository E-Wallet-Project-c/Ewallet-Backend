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
        Task<List<Wallet>> GetWalletsByUserId(int userId);
        Task<Wallet?> GetWalletwithtransactionByIdAsync(int walletId);
        Task<Wallet?> GetWalletByIdAsync(int walletId);

        Task<IEnumerable<Transaction>> GetWalletTransactionsAsync(int walletId);
        Task<Wallet> CreateWallet(Wallet wallet);

        Task<Wallet> DeleteWalletById(Wallet Wallet);
        Task<Wallet> DeleteWalletById(Wallet PrimaryWallet, Wallet SecondaryWallet);

        Task<Wallet> SetAsDefault(Wallet Wallet);

        Task<Wallet?> GetUserDefaultWallet(int userId);




    }
}
