using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Mappers;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Interfaces;
using E_wallet.Infrastrucure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Services
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly BeneficiaryMapper _beneficiaryMapper;
        private readonly IWalletRepository _walletRepository;
       public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, BeneficiaryMapper beneficiaryMapper, IWalletRepository walletRepository)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _beneficiaryMapper = beneficiaryMapper;
            _walletRepository = walletRepository;
        }
        public async Task<Result<BeneficiaryResponse>> CreateBeneficiary(BeneficiaryRequest beneficiary)
        {
            var existing = await _beneficiaryRepository.GetByBeneficiaryWalletIdAsync(beneficiary.BeneficiaryWalletId);
            if (existing != null)
            {
                return Result<BeneficiaryResponse>.Failure("Beneficiary already exists");
            }
            var Beneficiary = _beneficiaryMapper.ToEntity(beneficiary);
            await _beneficiaryRepository.AddAsync(Beneficiary);
            var response = _beneficiaryMapper.ToResponseBeneficiary(Beneficiary);
            return Result<BeneficiaryResponse>.Success(response);
        }




        public Task<Result> DeleteBeneficiary(int beneficiaryId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IEnumerable<BeneficiaryResponse>>> GetAllBeneficiariesByWalletId(int walletId)
        {
            var wallet = await _walletRepository.GetWalletByIdAsync(walletId);
            if (wallet == null)
                return Result<IEnumerable<BeneficiaryResponse>>.Failure("Wallet not found");

            var existing = await _beneficiaryRepository.GetAllBeneficiariesByWalletIdAsync(walletId);
            if (existing == null || !existing.Any())
                return Result<IEnumerable<BeneficiaryResponse>>.Failure("No beneficiaries found");

            var response = existing.Select(b => _beneficiaryMapper.ToResponseBeneficiary(b));

            return Result<IEnumerable<BeneficiaryResponse>>.Success(response);
        }

        public async Task<Result<BeneficiaryResponse>> GetBeneficiaryById(int beneficiaryId)
        {
          Beneficiary  existing=await _beneficiaryRepository.GetByIdAsync(beneficiaryId);
            if (existing == null)
            {
                return Result<BeneficiaryResponse>.Failure("Benificiary not found");
            }
            var response= _beneficiaryMapper.ToResponseBeneficiary(existing);
                return Result<BeneficiaryResponse>.Success(response);
        }

        public Task<Result<BeneficiaryResponse>> UpdateBeneficiary(BeneficiaryRequest beneficiary)
        {
            throw new NotImplementedException();
        }
    }
}
