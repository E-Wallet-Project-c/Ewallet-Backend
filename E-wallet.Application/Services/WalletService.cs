using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Mappers;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Enums;
using E_wallet.Domain.Interfaces;
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

        public WalletService(IWalletRepository walletRepo)
        {
            _walletRepo = walletRepo;
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

        public async Task<WalletResponse> CreateWallet(int UserId)
        {
            //get all wallets by user id
            //checking if user already has a wallet
            var wallet = await _walletRepo.GetWalletsByUserId(UserId);
            bool IsDefault=true;
            if (wallet.Count!=0)
            {
                IsDefault = false;
            }
                var savedWallet= await  _walletRepo.CreateWallet(new Wallet
                {
                    UserId = UserId,
                    Currency = "JD",
                    IsActive = true,
                    IsDeleted = false,
                    IsDefaultWallet= IsDefault,
                    CreatedAt = DateTime.Now,
                    CreatedBy=null
                });
                return new WalletResponse
                {
                    WalletId = savedWallet.Id,
                    userId = savedWallet.UserId,
                    Message = "Wallet created successfully."
                };
           
        }
       public async Task<List<WalletResponse>> GetUserWallets (int UserId)
        {
           
            //getting all wallets by user id
            var wallets = await _walletRepo.GetWalletsByUserId(UserId);
            if (wallets.Count == 0)
            {
                return new List<WalletResponse>();
            }
            //transforming from Wallet entity to response DTO and returning the list
            return wallets.Select(wallet => new WalletResponse
            {
                WalletId = wallet.Id,
                userId = wallet.UserId,
                Currency = wallet.Currency,
                IsDefaultWallet = wallet.IsDefaultWallet,
                IsDeleted = wallet.IsDeleted,
                IsActive = wallet.IsActive,
                UpdatedAt = wallet.UpdatedAt,
                CreatedAt = wallet.CreatedAt
            }).ToList();
        }

    } 
}
