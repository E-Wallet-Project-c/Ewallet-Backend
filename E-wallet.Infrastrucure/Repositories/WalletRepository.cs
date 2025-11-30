using E_wallet.Domain.Context;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Infrastrucure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _context;

        public WalletRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet?> GetWalletByIdAsync(int walletId, CancellationToken ct) {

            return await _context.Wallets.Where(w => w.Id == walletId && w.IsActive == true)
                .AsNoTracking()
                .SingleOrDefaultAsync(ct);
        
        }
        public async Task<Wallet?> GetWalletwithtransactionByIdAsync(int walletId)
        {
            return await _context.Wallets
             .Include(w => w.Transactions)
             .AsNoTracking()
             .FirstOrDefaultAsync(w => w.Id == walletId && w.IsActive == true && w.IsDeleted == false);
        }

        public async Task<IEnumerable<Transaction>> GetWalletTransactionsAsync(int walletId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(t => t.WalletId == walletId && t.IsActive == true)
                .ToListAsync();
        }
        public async Task<List<Wallet>> GetWalletsByUserId(int userId, CancellationToken ct)
        {
            return await _context.Wallets
                                 .AsNoTracking()         // read-only mode = faster & lighter
                                 .Where(w => w.UserId == userId && w.IsDeleted==false)  
                                 .ToListAsync(ct);
        }
        public async Task<Wallet> CreateWallet(Wallet wallet, CancellationToken ct)
        {
            await _context.Wallets.AddAsync(wallet,ct);
            await _context.SaveChangesAsync(ct);
            return wallet;
        }

        public async Task<Wallet> DeleteWalletById(Wallet Wallet, CancellationToken ct)
        {

            var wallet = await _context.Wallets.Where(w => w.Id == Wallet.Id && w.UserId == Wallet.UserId).SingleOrDefaultAsync(ct);

            if (wallet.IsDeleted == true || wallet == null|| wallet.IsDefaultWallet) 
            {
                return null;
            }       
            wallet.IsActive = false;
            wallet.IsDeleted = true;
            wallet.IsDefaultWallet= false;
            wallet.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync(ct);
            await DeleteRelatedItems(wallet,ct);
            return wallet;
        }
        public async Task<Wallet> DeleteWalletById(Wallet PrimaryWallet, Wallet SecondaryWallet, CancellationToken ct)
        {
            
            var _PrimaryWallet = await _context.Wallets.Where(w => w.Id == PrimaryWallet.Id && w.UserId == PrimaryWallet.UserId && w.IsDefaultWallet==true)
                .SingleOrDefaultAsync(ct);
            var _SecondaryWallet =await _context.Wallets.Where(w => w.Id == SecondaryWallet.Id && w.UserId == SecondaryWallet.UserId && w.IsActive==true && w.IsDefaultWallet == false)
                .SingleOrDefaultAsync(ct);
            if (_PrimaryWallet == null )
            {
                return null;
            }
            _PrimaryWallet.IsActive = false;
            _PrimaryWallet.IsDeleted = true;
            _PrimaryWallet.IsDefaultWallet= false;
            _PrimaryWallet.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            await DeleteRelatedItems(_PrimaryWallet, ct);
            
            return await SetAsDefault(SecondaryWallet, ct);
        }

   


     public async Task<Wallet> SetAsDefault(Wallet Wallet, CancellationToken ct)
        {
            Wallet? oldwallet = await _context.Wallets.Where(w => w.UserId==Wallet.UserId && w.IsDefaultWallet==true).FirstOrDefaultAsync(ct);
            Wallet? newwallet = await _context.Wallets.Where(w => w.Id == Wallet.Id).FirstOrDefaultAsync(ct);

            if (oldwallet == null || oldwallet.IsActive == true || newwallet == null || newwallet.IsActive == true)
            {
                return null;
            }
            newwallet.IsDefaultWallet = true;
            oldwallet.IsDefaultWallet = false;
            await _context.SaveChangesAsync(ct);
            return newwallet;
        }

        private async Task DeleteRelatedItems(Wallet wallet, CancellationToken ct)
        {
            var existingWallet = await _context.Wallets.Where(w=>w.Id==wallet.Id && w.UserId==wallet.UserId).FirstOrDefaultAsync(ct);
            if (existingWallet == null)
            {
                return ;
            }
            var Transactions = _context.Transactions.Where(t => t.WalletId == wallet.Id).ToList();

            foreach (Transaction item in Transactions) {
            item.IsActive = false;
            item.IsDeleted = true;
                await _context.SaveChangesAsync(ct);
            }
            var Beneficiaries = _context.Beneficiaries.Where(b => b.WalletId == wallet.Id || b.BeneficiaryWalletId == wallet.Id).ToList();

            foreach (Beneficiary item in Beneficiaries)
            {
                item.IsActive = false;
                item.IsDeleted = true;
                await _context.SaveChangesAsync(ct);
            }

            var Transfers = _context.Transfers.Where(tr => tr.SenderWalletId == wallet.Id || tr.ReciverWalletId == wallet.Id).ToList();

            foreach (Transfer item in Transfers)
            {
                item.IsActive = false;
                item.IsDeleted = true;
                await _context.SaveChangesAsync(ct);
            }

            var UserBankAccounts = _context.UserBankAccounts.Where(uba => uba.WalletId == wallet.Id).ToList();

            foreach (UserBankAccount item in UserBankAccounts)
            {
                item.IsActive = false;
                item.IsDeleted = true;
                await _context.SaveChangesAsync(ct);

            }
        }
    }
}
