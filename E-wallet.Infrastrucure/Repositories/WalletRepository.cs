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

        public async Task<Wallet?> GetWalletByIdAsync(int walletId) {

            return _context.Wallets.Where(w => w.Id == walletId && w.IsActive == true)
                .AsNoTracking()
                .SingleOrDefault();
        
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
        public async Task<List<Wallet>> GetWalletsByUserId(int userId)
        {
            return await _context.Wallets
                                 .AsNoTracking()         // read-only mode = faster & lighter
                                 .Where(w => w.UserId == userId && w.IsDeleted==false)  
                                 .ToListAsync();
        }
        public async Task<Wallet> CreateWallet(Wallet wallet)
        {
            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task<Wallet> DeleteWalletById(Wallet Wallet)
        {

            var wallet = await _context.Wallets.Where(w => w.Id == Wallet.Id && w.UserId == Wallet.UserId).SingleOrDefaultAsync();

            if (wallet.IsDeleted == true || wallet == null|| wallet.IsDefaultWallet) 
            {
                return null;
            }       
            wallet.IsActive = false;
            wallet.IsDeleted = true;
            wallet.IsDefaultWallet= false;
            wallet.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            await DeleteRelatedItems(wallet);
            return wallet;
        }
        public async Task<Wallet> DeleteWalletById(Wallet PrimaryWallet, Wallet SecondaryWallet)
        {
            
            var _PrimaryWallet = await _context.Wallets.Where(w => w.Id == PrimaryWallet.Id && w.UserId == PrimaryWallet.UserId && w.IsDefaultWallet==true).SingleOrDefaultAsync();
            var _SecondaryWallet =await _context.Wallets.Where(w => w.Id == SecondaryWallet.Id && w.UserId == SecondaryWallet.UserId && w.IsActive==true && w.IsDefaultWallet == false).SingleOrDefaultAsync();
            if (_PrimaryWallet == null )
            {
                return null;
            }
            _PrimaryWallet.IsActive = false;
            _PrimaryWallet.IsDeleted = true;
            _PrimaryWallet.IsDefaultWallet= false;
            _PrimaryWallet.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            await DeleteRelatedItems(_PrimaryWallet);
            
            return await SetAsDefault(SecondaryWallet);
        }

   


     public async Task<Wallet> SetAsDefault(Wallet Wallet)
        {
            Wallet? oldwallet = await _context.Wallets.Where(w => w.UserId==Wallet.UserId && w.IsDefaultWallet==true).FirstOrDefaultAsync();
            Wallet? newwallet = await _context.Wallets.Where(w => w.Id == Wallet.Id).FirstOrDefaultAsync();

            if (oldwallet == null || oldwallet.IsActive == true || newwallet == null || newwallet.IsActive == true)
            {
                return null;
            }
            newwallet.IsDefaultWallet = true;
            oldwallet.IsDefaultWallet = false;
            await _context.SaveChangesAsync();
            return newwallet;
        }

        private async Task DeleteRelatedItems(Wallet wallet)
        {
            var existingWallet = await _context.Wallets.Where(w=>w.Id==wallet.Id && w.UserId==wallet.UserId).FirstOrDefaultAsync();
            if (existingWallet == null)
            {
                return ;
            }
            var Transactions = _context.Transactions.Where(t => t.WalletId == wallet.Id).ToList();

            foreach (Transaction item in Transactions) {
            item.IsActive = false;
            item.IsDeleted = true;
            }
            var Beneficiaries = _context.Beneficiaries.Where(b => b.WalletId == wallet.Id || b.BeneficiaryWalletId == wallet.Id).ToList();

            foreach (Beneficiary item in Beneficiaries)
            {
                item.IsActive = false;
                item.IsDeleted = true;
            }

            var Transfers = _context.Transfers.Where(tr => tr.SenderWalletId == wallet.Id || tr.ReciverWalletId == wallet.Id).ToList();

            foreach (Transfer item in Transfers)
            {
                item.IsActive = false;
                item.IsDeleted = true;
            }

            var UserBankAccounts = _context.UserBankAccounts.Where(uba => uba.WalletId == wallet.Id).ToList();

            foreach (UserBankAccount item in UserBankAccounts)
            {
                item.IsActive = false;
                item.IsDeleted = true;
            }
        }
    }
}
