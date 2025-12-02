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
        Task<List<Wallet>> GetWalletsByUserId(int userId, int pagenumber, int max, CancellationToken ct);
        Task<Wallet?> GetWalletwithtransactionByIdAsync(int walletId);
        Task<Wallet?> GetWalletByIdAsync(int walletId, CancellationToken ct);

        Task<IEnumerable<Transaction>> GetWalletTransactionsAsync(int walletId);
        Task<Wallet> CreateWallet(Wallet wallet, CancellationToken ct);
        Task<Wallet> DeleteWalletById(int WalletId, int UserId, CancellationToken ct);
        Task<Wallet> DeleteDefaultWalletById(int UserId, int PrimaryWalletId, int SecondaryWalletId, CancellationToken ct);

        Task<Wallet> SetAsDefault(int WalletId,int UserId, CancellationToken ct);

        Task<Wallet?> GetUserDefaultWallet(int userId);




    }
}
