using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Interfaces
{
    public interface IBeneficiaryService
    {
     
            Task<BeneficiaryResponse>CreateBeneficiary(BeneficiaryRequest beneficiary);//done
            Task<Result<BeneficiaryResponse>> GetBeneficiaryById(int beneficiaryId);//done
            Task<Result<IEnumerable<BeneficiaryResponse>>> GetAllBeneficiariesByWalletId(int walletId);//done 
            Task<Result<BeneficiaryResponse>> UpdateBeneficiary(BeneficiaryRequest beneficiary);
            Task<Result> DeleteBeneficiary(int beneficiaryId);
        }



    
}
