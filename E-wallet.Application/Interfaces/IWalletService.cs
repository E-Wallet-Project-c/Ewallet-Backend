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
        Task<Result<WalletResponse>> CreateWallet(int UserId,WalletRequest NewWallet);
        Task<Result<WalletResponse>> GetUserWallets(WalletRequest NewWallet);
        Task<Result<TopUpWithdrawResponse>> TopUpToWalletAsync(TopUpWithdrawRequest dto);
        Task<Result<TopUpWithdrawResponse>> WithdrawFromWalletAsync(TopUpWithdrawRequest dto);
        Task<Result<TransferResponse>> TransferFromWalletAsync(TransferRequest dto);

    }

}
