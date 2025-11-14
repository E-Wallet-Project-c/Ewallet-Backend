using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Domain.Interfaces
{
    public interface IBeneficiaryRepository
    {
        Task AddAsync (Beneficiary beneficiary);
        Task <IEnumerable<Beneficiary>>GetAllAsync();
        Task<Beneficiary?> GetByIdAsync(int id);
        Task<Beneficiary?> GetByBeneficiaryWalletIdAsync(int BeneficiaryWalletId);
        Task<IEnumerable<Beneficiary?> >GetAllBeneficiariesByWalletIdAsync(int WalletId);

        Task<Beneficiary> UpdateAsync(Beneficiary beneficiary);
        Task DeleteAsync (int id);
    }
}
