using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Domain.Entities;
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
        Task<WalletResponse?> CreateWallet(WalletRequest newWallet, CancellationToken ct);
        Task<List<WalletResponse>> GetUserWallets(int UserId, int pagenumber, int Max, CancellationToken ct);
        Task<WalletResponse> GetWalletById(int Id, CancellationToken ct);
        Task<Result<TopUpWithdrawResponse>> TopUpToWalletAsync(TopUpWithdrawRequest dto,CancellationToken ct);
        Task<Result<TopUpWithdrawResponse>> WithdrawFromWalletAsync(TopUpWithdrawRequest dto, CancellationToken ct);
        Task<Result<TransferResponse>> TransferFromWalletAsync(TransferRequest dto, CancellationToken ct);
        Task<WalletResponse> DeleteDefaultWalletById(int UserId, int PrimaryWalletId, int SecondaryWalletId, CancellationToken ct);
        Task<WalletResponse> DeleteWalletById(int WalletId, int UserId, CancellationToken ct);
        Task<WalletResponse> SetDefaultWallet(int WalletId,int UserId, CancellationToken ct);
        Task<WalletResponse> GetUserDefaultWallet(int UserId, CancellationToken ct);

    }

}
