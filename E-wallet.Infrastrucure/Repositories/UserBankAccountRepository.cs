using E_wallet.Domain.Context;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Infrastrucure.Repositories
{
    public class UserBankAccountRepository : IUserBankAccountRepository
    {
        private readonly ApplicationDbContext _context;
        public UserBankAccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<UserBankAccount> AddAsync(UserBankAccount userBankAccount)
        {
            var userBank = await _context.UserBankAccounts.AddAsync(userBankAccount);
            await _context.SaveChangesAsync();
            userBankAccount.Id = userBank.Entity.Id;
            return userBankAccount;
        }


        public async Task<UserBankAccount> EditAsync(UserBankAccount userBankAccount)
        {
            _context.UserBankAccounts.Update(userBankAccount);
            await _context.SaveChangesAsync();
            return userBankAccount;

        }



        public async Task<UserBankAccount?> GetByIdAsync(int id)
        {
            return await _context.UserBankAccounts.FirstOrDefaultAsync(U => U.Id == id);
        }

        public async Task<List<UserBankAccount>> GetByWalletIdAsync(int walletId)
        {
            return await _context.UserBankAccounts
                .Include(u => u.Wallet)
                .AsNoTracking()
                .Where(u => u.WalletId == walletId)
                .ToListAsync();
        }



        public async Task<UserBankAccount> UpdateStatusByIdAsync(int Id, bool status)
        {
            Console.WriteLine("Id received in UpdateStatusByIdAsync: " + Id);

            // Fetch the entity
            var userBankAccount  =await _context.UserBankAccounts.FirstOrDefaultAsync(U => U.Id == Id);

            //Update the status
            userBankAccount.IsActive = status; // assuming you have a Status property

            //Save changes
            await _context.SaveChangesAsync();

            //Return the updated entity
            return userBankAccount;

        }

    }
}


