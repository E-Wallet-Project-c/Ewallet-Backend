using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Dtos.SignalR;          // 👈 add this
using E_wallet.Application.Interfaces;
using E_wallet.Application.Mappers;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Enums;
using E_wallet.Domain.Interfaces;
using E_wallet.Infrastrucure.Repositories;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Http;

namespace E_wallet.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepo;
        private readonly ITransactionRepository _transactionRepo;
        private readonly IUserBankAccountRepository _userBankAccountRepo;
        private readonly ITransferRepository _transferRepository;
        private readonly ILimitRepository _limitRepository;
        private readonly INotificationService _notifications;
        private readonly IHubContext<E_wallet.Applications.Hubs.AppHub> _hubContext;
        private readonly IUserRepository _userRepository;
        public WalletService(
            IWalletRepository walletRepo,
            ITransactionRepository transactionRepo,
            IUserBankAccountRepository userBankAccountRepo,
            ITransferRepository transferRepository,
            ILimitRepository limitRepository,
            INotificationService notifications,
            IHubContext<E_wallet.Applications.Hubs.AppHub> hubContext,
            IUserRepository userRepository)
        {
            _walletRepo = walletRepo;
            _transactionRepo = transactionRepo;
            _userBankAccountRepo = userBankAccountRepo;
            _transferRepository = transferRepository;
            _limitRepository = limitRepository;
            _notifications = notifications;
            _hubContext = hubContext;
            _userRepository = userRepository;
        }

        public async Task<WalletBalanceResponseDto?> GetWalletBalanceAsync(int walletId)
        {
            var wallet = await _walletRepo.GetWalletwithtransactionByIdAsync(walletId);
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

        public async Task<WalletResponse?> CreateWallet(WalletRequest newWallet)
        {

            var user = await _userRepository.GetByIdAsync(newWallet.UserId);

            if (user == null)
                return null;


            var savedWallet = await _walletRepo.CreateWallet(WalletMapper.ToEntity(newWallet));

            // Notification
            await _notifications.AddAndSendAsync(new NotificationRequest
            {
                UserId = newWallet.UserId,
                Content = "You have created a wallet successfully.",
                Event = "Wallet Creation",
                Type = "InApp"
            });



            return WalletMapper.ToResponse(savedWallet);
        }
        public async Task<WalletResponse> GetWalletById(int Id)
        {
            Wallet wallet = await _walletRepo.GetWalletByIdAsync(Id);
            if (wallet == null)
            {
                return null;
            }

            return WalletMapper.ToResponse(wallet);
        }
        public async Task<List<WalletResponse>> GetUserWallets(int UserId)
        {
            var user = await _userRepository.GetByIdAsync(UserId);

            if (user == null)
                return null;

            var wallets = await _walletRepo.GetWalletsByUserId(UserId);
            if (wallets.Count == 0)
            {
                return null;
            }

            return WalletMapper.ToListResponse(wallets);

        }

        public async Task<WalletResponse> DeleteWalletById(WalletRequest Wallet)
        {
            var user = await _userRepository.GetByIdAsync(Wallet.UserId);
            if (user == null)
                return null;

            Wallet wallet = await _walletRepo.DeleteWalletById(WalletMapper.ToEntity(Wallet));

            if (wallet == null)
            {
                return null;
            }
            return WalletMapper.ToResponse(wallet);

        }


        public async Task<WalletResponse> DeleteDefaultWalletById(DefaultWalletDeleteRequest Wallet)
        {
            var user = await _userRepository.GetByIdAsync(Wallet.UserId);
            if (user == null)
                return null;

            WalletRequest PrimaryWallet = new WalletRequest
            {
                UserId = Wallet.UserId,
                WalletId = Wallet.PrimaryWalletId
            };
            WalletRequest SecondaryWallet = new WalletRequest
            {
                UserId = Wallet.UserId,
                WalletId = Wallet.PrimaryWalletId
            };


            Wallet wallet = await _walletRepo.DeleteWalletById(WalletMapper.ToEntity(PrimaryWallet), WalletMapper.ToEntity(SecondaryWallet));

            if (wallet == null)
            {
                return null;
            }
            return WalletMapper.ToResponse(wallet);
        }



        public async Task<WalletResponse> SetDefaultWallet(WalletRequest Wallet)
        {
            var user = await _userRepository.GetByIdAsync(Wallet.UserId);
            if (user == null)
                return null;
            var Wallet_ = WalletMapper.ToEntity(Wallet);
            var wallet = await _walletRepo.SetAsDefault(Wallet_);
            if (wallet == null)
            {
                return null;
            }
            return WalletMapper.ToResponse(wallet);
        }


        #region TopUpToWalletAsync

        public async Task<Result<TopUpWithdrawResponse>> TopUpToWalletAsync(TopUpWithdrawRequest dto)
        {
            Wallet wallet = await _walletRepo.GetWalletwithtransactionByIdAsync(dto.WalletId);
            if (wallet == null)
                return Result<TopUpWithdrawResponse>.Failure("Wallet not exist");

            UserBankAccount userBankAcc = await _userBankAccountRepo.GetByIdAsync(dto.UserBankAccountId);
            if (userBankAcc == null)
                return Result<TopUpWithdrawResponse>.Failure("User bank account not exist");

            if (userBankAcc.Balance < dto.Balance)
                return Result<TopUpWithdrawResponse>.Failure("Insufficient bank account balance");

            // Add credit transaction
            await _transactionRepo.AddAsync(new Transaction
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

            // Edit bank balance
            double bankAccBalance = userBankAcc.Balance;
            bankAccBalance -= dto.Balance;
            userBankAcc.Balance = bankAccBalance;
            await _userBankAccountRepo.EditAsync(userBankAcc);

            // Notification
            await _notifications.AddAndSendAsync(new NotificationRequest
            {
                UserId = wallet.UserId,
                Content = $"Your wallet has been topped up with {dto.Balance}. New wallet balance is {walletBalance}.",
                Event = "Wallet Top-Up",
                Type = "InApp"
            });



            return Result<TopUpWithdrawResponse>.Success(new TopUpWithdrawResponse
            {
                UserBankAccountId = dto.UserBankAccountId,
                NewBalance = walletBalance
            });
        }

        #endregion

        #region WithdrawFromWalletAsync

        public async Task<Result<TopUpWithdrawResponse>> WithdrawFromWalletAsync(TopUpWithdrawRequest dto)
        {
            Wallet wallet = await _walletRepo.GetWalletwithtransactionByIdAsync(dto.WalletId);
            if (wallet == null)
                return Result<TopUpWithdrawResponse>.Failure("Wallet not exist");

            UserBankAccount userBankAcc = await _userBankAccountRepo.GetByIdAsync(dto.UserBankAccountId);
            if (userBankAcc == null)
                return Result<TopUpWithdrawResponse>.Failure("User bank account not exist");

            var transactions = await _walletRepo.GetWalletTransactionsAsync(dto.WalletId);
            double walletBalance = transactions.Sum(t => t.Type == TransactionType.Credit ? t.Amount : -t.Amount);

            if (walletBalance < dto.Balance)
                return Result<TopUpWithdrawResponse>.Failure("Insufficient wallet balance");

            // Add debit transaction
            await _transactionRepo.AddAsync(new Transaction
            {
                WalletId = dto.WalletId,
                Amount = dto.Balance,
                Action = TransactionAction.Withdraw,
                Type = TransactionType.Debit,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = null
            });

            // Update wallet balance
            var transactionsUpdated = await _walletRepo.GetWalletTransactionsAsync(dto.WalletId);
            double newWalletBalance = transactionsUpdated.Sum(t => t.Type == TransactionType.Credit ? t.Amount : -t.Amount);

            // Update bank balance
            double bankAccBalance = userBankAcc.Balance;
            bankAccBalance += dto.Balance;
            userBankAcc.Balance = bankAccBalance;
            await _userBankAccountRepo.EditAsync(userBankAcc);

            // Notification
            await _notifications.AddAndSendAsync(new NotificationRequest
            {
                UserId = wallet.UserId,
                Content = $"You have withdrawn {dto.Balance} from your wallet. New wallet balance is {newWalletBalance}.",
                Event = "Wallet Withdrawal",
                Type = "InApp"
            });

            return Result<TopUpWithdrawResponse>.Success(new TopUpWithdrawResponse
            {
                UserBankAccountId = dto.UserBankAccountId,
                NewBalance = newWalletBalance
            });
        }

        #endregion

        #region TransferFromWalletAsync

        public async Task<Result<TransferResponse>> TransferFromWalletAsync(TransferRequest dto)
        {
            try
            {
                if (ExceedNumberOfDailyLimit(dto.SenderWalletId, DateTime.Now))
                    return Result<TransferResponse>.Failure("Daily number of transfer limit exceeded for the sender wallet.");

                if (ExceedDailyAmountLimit(dto.SenderWalletId, DateTime.Now, dto.Amount))
                    return Result<TransferResponse>.Failure("Daily amount transfer limit exceeded for the sender wallet.");

                var senderWallet = await _walletRepo.GetWalletwithtransactionByIdAsync(dto.SenderWalletId);
                if (senderWallet == null)
                    return Result<TransferResponse>.Failure("Sender wallet not exist");

                var receiverWallet = await _walletRepo.GetWalletwithtransactionByIdAsync(dto.ReceiverWalletId);
                if (receiverWallet == null)
                    return Result<TransferResponse>.Failure("Receiver wallet not exist");

                var senderTransactions = await _walletRepo.GetWalletTransactionsAsync(dto.SenderWalletId);
                double senderBalance = senderTransactions.Sum(t => t.Type == TransactionType.Credit ? t.Amount : -t.Amount);

                if (senderBalance < dto.Amount)
                    return Result<TransferResponse>.Failure("Insufficient amount in the sender balance");

                // Add transfer
                Transfer transferReturned = await _transferRepository.AddTransfer(TransferMapper.ToTransferEntity(dto));
                if (transferReturned == null)
                    return Result<TransferResponse>.Failure("Transfer failed");

                // Add transactions
                await _transactionRepo.AddAsync(TransactionMapper.ToSenderEntity(transferReturned.Id, dto));
                await _transactionRepo.AddAsync(TransactionMapper.ToReceiverEntity(transferReturned.Id, dto));
                await _transactionRepo.AddAsync(TransactionMapper.ToFeeEntity(transferReturned.Id, dto));

                // Recalculate balances
                var updatedSenderTransactions = await _walletRepo.GetWalletTransactionsAsync(dto.SenderWalletId);
                double newSenderBalance = updatedSenderTransactions.Sum(t => t.Type == TransactionType.Credit ? t.Amount : -t.Amount);

                var updatedReceiverTransactions = await _walletRepo.GetWalletTransactionsAsync(dto.ReceiverWalletId);
                double newReceiverBalance = updatedReceiverTransactions.Sum(t => t.Type == TransactionType.Credit ? t.Amount : -t.Amount);

                // Notifications
                await _notifications.AddAndSendAsync(new NotificationRequest
                {
                    UserId = senderWallet.UserId,
                    Content = $"You have sent {dto.Amount} to wallet #{dto.ReceiverWalletId}. New balance: {newSenderBalance}.",
                    Event = "Transfer Sent",
                    Type = "InApp"
                });

                await _notifications.AddAndSendAsync(new NotificationRequest
                {
                    UserId = receiverWallet.UserId,
                    Content = $"You have received {dto.Amount} from wallet #{dto.SenderWalletId}. New balance: {newReceiverBalance}.",
                    Event = "Money Received",
                    Type = "InApp"
                });

                await _hubContext.Clients
                    .User(receiverWallet.UserId.ToString())
                    .SendAsync("WalletUpdated", new WalletUpdatedDTO
                    {
                        UserId = receiverWallet.UserId,
                        WalletId = receiverWallet.Id,
                        Balance = newReceiverBalance,
                    });



                await _hubContext.Clients
                    .User(receiverWallet.UserId.ToString())
                    .SendAsync("MoneyReceived", new MoneyReceivedDTO
                    {
                        ReceiverUserId = receiverWallet.UserId,
                        ReceiverWalletId = receiverWallet.Id,
                        SenderUserId = senderWallet.UserId,
                        Amount = dto.Amount,
                        TransferId = transferReturned.Id
                    });

                return Result<TransferResponse>.Success(TransferMapper.ToTransferResponse(transferReturned.Id));
            }
            catch (Exception ex)
            {
                return Result<TransferResponse>.Failure("Transfer failed: " + ex.Message);
            }
        }

        #endregion

        #region CheckDailyLimit

        private bool ExceedNumberOfDailyLimit(int walletId, DateTime date)
        {
            var dailyLimitTask = _limitRepository.GetLimitsByTypeAndScopeAsync(LimitType.TRANSACTION_COUNT, LimitScope.DAILY);
            if (dailyLimitTask == null)
                return false;

            var dailyLimit = dailyLimitTask.Result;
            if (dailyLimit == null)
                return false;

            int transactionsCount = _transactionRepo.GetAllByDay(walletId, date);
            return transactionsCount >= dailyLimit.Value;
        }

        #endregion

        #region CheckDailyAmountLimit

        private bool ExceedDailyAmountLimit(int walletId, DateTime date, double amount)
        {
            var dailyAmountLimitTask = _limitRepository.GetLimitsByTypeAndScopeAsync(LimitType.AMOUNT, LimitScope.DAILY);
            if (dailyAmountLimitTask == null)
                return false;

            var dailyAmountLimit = dailyAmountLimitTask.Result;
            if (dailyAmountLimit == null)
                return false;

            decimal? totalAmount = _transactionRepo.GetTotalAmountByDay(walletId, date);
            if (!totalAmount.HasValue)
                totalAmount = 0;

            return ((double?)totalAmount + amount) > dailyAmountLimit.Value;
            //
            //
        }

        #endregion

        #region GetUserDefaultWallet
        public async Task<WalletResponse> GetUserDefaultWallet(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return null;

            var wallet = await _walletRepo.GetUserDefaultWallet(userId);
            if (wallet == null)
                return null;

            return WalletMapper.ToResponse(wallet);
        }

        #endregion
    }
}
