using E_wallet.Domain.Context;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Infrastrucure.Repositories
{
    public class BeneficiaryRepository : IBeneficiaryRepository
    {
        private readonly ApplicationDbContext _context;
        public BeneficiaryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task AddAsync(Beneficiary beneficiary)
        {
         _context.Beneficiaries.Add(beneficiary);
         return _context.SaveChangesAsync();
     
        }

        public async Task DeleteAsync(int id)
        {
            _context.Beneficiaries.Remove(_context.Beneficiaries.Find(id)!);
        }

        

        public async Task<IEnumerable<Beneficiary>> GetAllAsync()
        {
           return await _context.Beneficiaries.AsNoTracking().ToListAsync();
        }

        public Task<Beneficiary?> GetByIdAsync(int id)
        {
            return _context.Beneficiaries
                            .AsNoTracking()
                            .FirstOrDefaultAsync(b => b.Id == id);
        }

        public Task<Beneficiary?> GetByBeneficiaryWalletIdAsync(int BeneficiaryWalletid)
        {
            return _context.Beneficiaries
                            .AsNoTracking()
                            .FirstOrDefaultAsync(b => b.BeneficiaryWalletId== BeneficiaryWalletid);
        }

        public async  Task<Beneficiary> UpdateAsync(Beneficiary beneficiary)
        {
          _context.Beneficiaries.Update(beneficiary);
         _context.SaveChanges();
            return beneficiary;
        }

        public async Task<IEnumerable<Beneficiary?>> GetAllBeneficiariesByWalletIdAsync(int WalletId)
        {

          return  await _context.Beneficiaries
                            .AsNoTracking()
                            .Where(b => b.WalletId == WalletId).ToListAsync();
            
        }
    }
}
