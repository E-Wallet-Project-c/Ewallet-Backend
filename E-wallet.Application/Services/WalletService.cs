using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Mappers;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Enums;
using E_wallet.Domain.Interfaces;
using E_wallet.Infrastrucure.Repositories;
using Microsoft.AspNetCore.Server.IISIntegration;
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
        private readonly ITransactionRepository _transactionRepo;
        private readonly IUserBankAccountRepository _userBankAccountRepo;

        public WalletService(IWalletRepository walletRepo, ITransactionRepository transactionRepo, IUserBankAccountRepository userBankAccountRepo)
        {
            _walletRepo = walletRepo;
            _transactionRepo = transactionRepo;
            _userBankAccountRepo = userBankAccountRepo;

        }
        public async Task<WalletBalanceResponseDto?> GetWalletBalanceAsync(int walletId)
        {

            var wallet = await _walletRepo.GetWalletByIdAsync(walletId);
            if (wallet == null) 
                return null;

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



        public async Task<Result<TopUpWithdrawResponse>> TopUpToWalletAsync(TopUpWithdrawRequest dto)
        {
            //here we want to topup the wallet from the bank account , so we will add the balance to wallet 
            //also need to add this as topup transaction

            Wallet wallet = await _walletRepo.GetWalletByIdAsync(dto.WalletId);
            if (wallet == null)
            return Result<TopUpWithdrawResponse>.Failure("Wallet Not Exist");

            UserBankAccount userBankAcc = await _userBankAccountRepo.GetByIdAsync(dto.UserBankAccountId);
            if (userBankAcc == null)
                return Result<TopUpWithdrawResponse>.Failure("userBankAcc Not Exist");

            if (userBankAcc.Balance < dto.Balance)
                return Result<TopUpWithdrawResponse>.Failure("Insufficient bank account balance");

            //add transaction for adding balance to wallet  
            var trans = await  _transactionRepo.AddAsync(new Transaction
            {
                WalletId = dto.WalletId,
                Amount = dto.Balance,
                Action = TransactionAction.TopUp,
                Type = TransactionType.Credit,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = null
            });
            var transactions = await _walletRepo.GetWalletTransactionsAsync(dto.WalletId);
            double walletBalance = transactions.Sum(t => t.Type == TransactionType.Credit ? t.Amount : -t.Amount);
            
            //edit bank balance 
            double BankAccBalance = userBankAcc.Balance;
            BankAccBalance -= dto.Balance;
            userBankAcc.Balance = BankAccBalance;
            var userBankAccNew = await _userBankAccountRepo.EditAsync(userBankAcc);


            return Result<TopUpWithdrawResponse>.Success(new TopUpWithdrawResponse
            {
                UserBankAccountId = dto.UserBankAccountId,
                NewBalance = walletBalance            });

        }

        public async Task<Result<TopUpWithdrawResponse>> WithdrawFromWalletAsync(TopUpWithdrawRequest dto)
        {
            //here we want to withdraw from wallet to the bank account , so we will decrease balance from wallet 
            //also need to add this as withdraw transaction


            Wallet wallet = await _walletRepo.GetWalletByIdAsync(dto.WalletId);
            if (wallet == null)
                return Result<TopUpWithdrawResponse>.Failure("Wallet Not Exist");

            UserBankAccount userBankAcc = await _userBankAccountRepo.GetByIdAsync(dto.UserBankAccountId);
            if (userBankAcc == null)
                return Result<TopUpWithdrawResponse>.Failure("userBankAcc Not Exist");

            var transactions = await _walletRepo.GetWalletTransactionsAsync(dto.WalletId);
            double walletBalance = transactions.Sum(t => t.Type == TransactionType.Credit ? t.Amount : -t.Amount);
            if (walletBalance < dto.Balance)
                return Result<TopUpWithdrawResponse>.Failure("Insufficient bank account balance");
            //add transaction to decrease from wallet balance 
            var trans =await  _transactionRepo.AddAsync(new Transaction
            {
                WalletId = dto.WalletId,
                Amount = dto.Balance,
                Action = TransactionAction.Withdraw,
                Type = TransactionType.Debit,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = null
            });

       
            //edit bank balance 
            double BankAccBalance = userBankAcc.Balance;
            BankAccBalance += dto.Balance;
            userBankAcc.Balance = BankAccBalance;

            var userBankAccNew = await _userBankAccountRepo.EditAsync(userBankAcc);

            return Result<TopUpWithdrawResponse>.Success(new TopUpWithdrawResponse
            {
                UserBankAccountId = dto.UserBankAccountId,
                NewBalance = walletBalance
            });

        }

    }
}
