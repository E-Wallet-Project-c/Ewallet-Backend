using E_wallet.Domain.Context;
using E_wallet.Domain.Interfaces;

namespace E_wallet.Infrastrucure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IWalletRepository Wallets { get; private set; }
        public IUserRepository Users { get; private set; }
        public IUserBankAccountRepository UserBankAccounts { get; private set; }
        public ITransferRepository Transfers { get; private set; }
        public ITransactionRepository Transactions { get; private set; }
        public ISessionRepository Sessions { get; private set; }
        public IProfileRepository Profiles { get; private set; }
        public ILimitRepository Limits { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Wallets = new WalletRepository(_context);
            Users = new UserRepository(_context);
            UserBankAccounts = new UserBankAccountRepository(_context);
            Transfers = new TransferRepository(_context);
            Transactions = new TransactionRepository(_context);
            Sessions = new SessionRepository(_context);
            Profiles = new ProfileRepository(_context);
            Limits = new LimitRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}