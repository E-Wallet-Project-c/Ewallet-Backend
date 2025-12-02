using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Domain.Interfaces
{
    public interface IUserBankAccountRepository
    {
        Task<UserBankAccount> AddAsync(UserBankAccount userBankAccount);
        Task<UserBankAccount> EditAsync(UserBankAccount userBankAccount);
        Task<UserBankAccount?> GetByIdAsync(int id);
        Task<List<UserBankAccount>?> GetByWalletIdAsync(int walletId);
        Task<UserBankAccount?> UpdateStatusByIdAsync(int Id, bool status);


    }
}
