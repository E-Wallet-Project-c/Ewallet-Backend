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
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Services
{
    public class UserBankAccountService : IUserBankAccountService
    {

        private readonly IUserBankAccountRepository _userBankAccountRepository;
        public UserBankAccountService(IUserBankAccountRepository userBankAccountRepository)
        {
            _userBankAccountRepository = userBankAccountRepository;

        }

        public async Task<Result<UserBankAccountResponse>> CreateBankAsync(UserBankAccountRequest dto)
        {
            //check that account number not exist with this wallet ??

            var savedUserBankAccount = await _userBankAccountRepository.AddAsync(UserBankAccountMapper.ToEntity(dto));
            return Result<UserBankAccountResponse>.Success(UserBankAccountMapper.toResponse(savedUserBankAccount));

        }

        public async Task<Result<List<UserBankAccountResponse>>> GetAllByWalletIdAsync(int walletId)
        {
            var userBankAccounts = await _userBankAccountRepository.GetByWalletIdAsync(walletId);
            var responseList = new List<UserBankAccountResponse>();

            foreach (var userBankAcc in userBankAccounts)
            {
                responseList.Add(UserBankAccountMapper.toResponse(userBankAcc));
            }

            return Result<List<UserBankAccountResponse>>.Success(responseList);

        }


        public async Task<Result<UserBankAccountResponse>> UpdateStatusAsync(UpdateUserBankAccountRequest dto)
        {
            var userBankAccounts = await _userBankAccountRepository.GetByIdAsync(dto.Id);
            if (userBankAccounts==null)
            {
                return Result<UserBankAccountResponse>.Failure("user bank account not exist");
            }
            await _userBankAccountRepository.UpdateStatusByIdAsync(dto.Id, dto.IsActive);
            return Result<UserBankAccountResponse>.Success(UserBankAccountMapper.toResponse(userBankAccounts));

        }


    }
}
