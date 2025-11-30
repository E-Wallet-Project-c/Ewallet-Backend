using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IWalletRepository Wallets { get; }
        IUserRepository Users { get; }
        IUserBankAccountRepository UserBankAccounts { get; }
        ITransferRepository Transfers { get; }
        ITransactionRepository Transactions { get; }
        ISessionRepository Sessions { get; }
        IProfileRepository Profiles { get; }
        ILimitRepository Limits { get; }

        Task<int> CompleteAsync();
    }
}