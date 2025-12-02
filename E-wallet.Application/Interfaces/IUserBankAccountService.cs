using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace E_wallet.Application.Interfaces
{
    public interface IUserBankAccountService
    {
        Task<Result<UserBankAccountResponse>> CreateBankAsync(UserBankAccountRequest dto);
        Task<Result<List<UserBankAccountResponse>>> GetAllByWalletIdAsync(int walletId);

        Task<Result<UserBankAccountResponse>> UpdateStatusAsync(UpdateUserBankAccountRequest dto);




    }
}
