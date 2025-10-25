using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Mappers;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Enums;
using E_wallet.Domain.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepo;

        private readonly WalletMapper _mapper;

        public WalletService(IWalletRepository walletRepo, WalletMapper mapper)
        {
            _walletRepo = walletRepo;
            _mapper = mapper;
        }
        public async Task<WalletBalanceResponseDto?> GetWalletBalanceAsync(int walletId)
        {

            var wallet = await _walletRepo.GetWalletByIdAsync(walletId);
            if (wallet == null) return null;

            var transactions = await _walletRepo.GetWalletTransactionsAsync(walletId);

            double balance = transactions.Sum(t => t.Type == TransactionType.Credit ? t.Amount : -t.Amount);

            return new WalletBalanceResponseDto
            {
                WalletId = wallet.Id,
                Balance = balance,
                Currency = wallet.Currency ?? "JD",
                LastUpdated = wallet.UpdatedAt ?? wallet.CreatedAt ?? DateTime.UtcNow
            };

        }

        public async Task<Result<WalletResponse>> CreateWallet(WalletRequest NewWallet)
        {
            //get all wallets by user id
            //checking if user already has a wallet
            var wallet = await _walletRepo.GetWalletsByUserId(NewWallet.UserId);
            NewWallet.IsDefault = true;
            if (wallet.Count!=0)
            {
                NewWallet.IsDefault = false;
            }

            if (_walletRepo.GetWalletsByUserId(NewWallet.UserId) !=null)
            {
               return Result<WalletResponse>.Failure("Wallet is not exist");
            }
            var savedWallet = await _walletRepo.CreateWallet(_mapper.ToEntity(NewWallet.UserId, NewWallet.IsDefault));

            return Result<WalletResponse>.Success(_mapper.ToResponse(savedWallet)); 

        }
       public async Task<Result<WalletResponse>> GetUserWallets (WalletRequest NewWallet)
        {
           
            //getting all wallets by user id
            var wallets = await _walletRepo.GetWalletsByUserId(NewWallet.UserId);
            if (wallets.Count == 0)
            {
                return Result<WalletResponse>.Failure("User has no wallets");
            }
            //transforming from Wallet entity to response DTO and returning the list
           
            return Result<WalletResponse>.Success(_mapper.ToListResponse(wallets));

        }

    } 
}
