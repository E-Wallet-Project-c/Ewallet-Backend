using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Mappers
{
    public class BeneficiaryMapper
    {

        public  Beneficiary ToEntity(BeneficiaryRequest beneficiary)
        {
            return new Beneficiary
            {
                WalletId = beneficiary.WalletId,
                BeneficiaryWalletId = beneficiary.BeneficiaryWalletId,
                NickName = beneficiary.NickName,
                Purpos = beneficiary.Purpos,
                IsActive = true,
                CreatedAt = TimeOnly.FromDateTime(DateTime.UtcNow),
                CreatedBy = "System" 
            };
        }

        public BeneficiaryResponse ToResponseBeneficiary(Beneficiary beneficiary)
        {
       return new BeneficiaryResponse
            {
                Id = beneficiary.Id,
                BeneficiaryWalletId = beneficiary.BeneficiaryWalletId,
                NickName = beneficiary.NickName,
              
            };
        }

    }
}
