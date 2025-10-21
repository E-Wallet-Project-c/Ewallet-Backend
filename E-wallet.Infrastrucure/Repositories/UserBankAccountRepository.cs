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
    public class UserBankAccountRepository:IUserBankAccountRepository
    {
        private readonly ApplicationDbContext _context;
        public UserBankAccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }
       

        public async Task<UserBankAccount> AddAsync(UserBankAccount userBankAccount)
        {
            _context.UserBankAccounts.Add(userBankAccount);
            _context.SaveChangesAsync();
            return userBankAccount;
        }
    }

}
