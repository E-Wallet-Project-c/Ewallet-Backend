using E_wallet.Application.Dtos.Request;
using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Mappers
{
    public class TransactionMapper
    {
        public static Transaction ToSenderEntity(int transferId,TransferRequest dto)
        {
            return new Transaction
            {
                WalletId = dto.SenderWalletId,
                Amount=dto.Amount,
                TransferId=transferId,
                Type=Domain.Enums.TransactionType.Debit,
                Action=Domain.Enums.TransactionAction.Send,
                IsActive=true,
                CreatedAt=DateTime.Now

            };
        }

        public static Transaction ToReceiverEntity(int transferId, TransferRequest dto)
        {
            return new Transaction
            {
                WalletId = dto.ReceiverWalletId,
                Amount = dto.Amount,
                TransferId = transferId,
                Type = Domain.Enums.TransactionType.Credit,
                Action = Domain.Enums.TransactionAction.Receive,
                IsActive = true,
                CreatedAt = DateTime.Now

            };
        }

        public static Transaction ToFeeEntity(int transferId, TransferRequest dto)
        {
            return new Transaction
            {
                WalletId = dto.SenderWalletId,
                Amount = dto.TransferFee,
                TransferId = transferId,
                Type = Domain.Enums.TransactionType.Debit,
                Action = Domain.Enums.TransactionAction.Fee,
                IsActive = true,
                CreatedAt = DateTime.Now

            };
        }

    }
}
