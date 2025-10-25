using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Interfaces
{
    public interface IWalletService
    {
        Task<WalletBalanceResponseDto?> GetWalletBalanceAsync(int walletId);
        Task<Result<WalletResponse>> CreateWallet(WalletRequest NewWallet);
        Task<Result<WalletResponse>> GetUserWallets(WalletRequest NewWallet);
    }

}
