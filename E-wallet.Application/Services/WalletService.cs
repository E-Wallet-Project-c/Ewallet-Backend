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
        private readonly WalletMapper _mapper;
        private readonly ITransferRepository _transferRepository;
        public WalletService(IWalletRepository walletRepo, WalletMapper mapper, ITransactionRepository transactionRepo, IUserBankAccountRepository userBankAccountRepo, ITransferRepository transferRepository)
        {
            _walletRepo = walletRepo;
            _mapper = mapper;
            _transactionRepo = transactionRepo;
            _userBankAccountRepo = userBankAccountRepo;
            _transferRepository = transferRepository;

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

        public async Task<Result<WalletResponse>> CreateWallet(int UserId,WalletRequest NewWallet)
        {
            //get all wallets by user id
            //checking if user already has a wallet
            var wallet = await _walletRepo.GetWalletsByUserId(UserId);
            NewWallet.IsDefault = true;
            if (wallet.Count != 0)
            {
                NewWallet.IsDefault = false;
            }

            if (wallet.Count != 0)
            {
                return Result<WalletResponse>.Failure("User Already have Wallet");
            }
            NewWallet.UserId = UserId;
            var savedWallet = await _walletRepo.CreateWallet(_mapper.ToEntity(NewWallet.UserId, NewWallet.IsDefault));

            return Result<WalletResponse>.Success(_mapper.ToResponse(savedWallet));

        }
        public async Task<Result<WalletResponse>> GetUserWallets(WalletRequest NewWallet)
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
            var trans = await _transactionRepo.AddAsync(new Transaction
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
                NewBalance = walletBalance
            });

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
            var trans = await _transactionRepo.AddAsync(new Transaction
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

        public async Task<Result<TransferResponse>> TransferFromWalletAsync(TransferRequest dto)
        {
            try
            {
                //here in the transfer we will send amount from sender to receiver , and add these two steps as transaction ,also we should add the fee as transaction
                //1-add to transfer table 
                //2-add to transaction table (receiver transaction , sender transaction , fee transaction)

                //check the sender wallet id 
                var senderWallet = await _walletRepo.GetWalletByIdAsync(dto.SenderWalletId);
                if (senderWallet == null)
                    return Result<TransferResponse>.Failure("sender wallet not exist");
                //check receiver wallet id
                var receiverWallet = await _walletRepo.GetWalletByIdAsync(dto.ReceiverWalletId);
                if (receiverWallet == null)
                    return Result<TransferResponse>.Failure("receiver wallet not exist");
                //check if the sender wallet have the required amount balance 
                var transactions = await _walletRepo.GetWalletTransactionsAsync(dto.SenderWalletId);
                double walletBalance = transactions.Sum(t => t.Type == TransactionType.Credit ? t.Amount : -t.Amount);
                if (walletBalance < dto.Amount)
                    return Result<TransferResponse>.Failure("Insufficient amount in the sender balance");

                //add to transfer table
                Transfer transferReterned = await _transferRepository.AddTransfer(TransferMapper.ToTransferEntity(dto));
                if (transferReterned == null)
                    return Result<TransferResponse>.Failure("transfer failed");

                //add to transaction table 
                //1-add the sender transaction
                await _transactionRepo.AddAsync(TransactionMapper.ToSenderEntity(transferReterned.Id, dto));
                //2-add the receiver transaction
                await _transactionRepo.AddAsync(TransactionMapper.ToReceiverEntity(transferReterned.Id, dto));
                //2-add the fee transaction
                await _transactionRepo.AddAsync(TransactionMapper.ToFeeEntity(transferReterned.Id, dto));

                return Result<TransferResponse>.Success(TransferMapper.ToTransferResponse(transferReterned.Id));

            }
            catch (Exception ex)
            {

                return Result<TransferResponse>.Failure("Transfer failed: " + ex.Message);

            }

        }

    }
}
