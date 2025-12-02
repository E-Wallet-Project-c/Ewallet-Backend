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
    public class UserBankAccountMapper
    {
        public static UserBankAccount ToEntity(UserBankAccountRequest dto)
        {
            return new UserBankAccount
            {
                BankName = dto.BankName,
                AccountNumber = dto.AccountNumber,
                WalletId = dto.WalletId,
                Balance = dto.Balance,
                IsActive = true,
                CreatedAt = TimeOnly.FromDateTime(DateTime.UtcNow),
                CreatedBy = "System"
            };
        }

        public static UserBankAccountResponse toResponse(UserBankAccount entity)
        {
            return new UserBankAccountResponse
            {
                Id = entity.Id,
                BankName=entity.BankName,
                Balance=entity.Balance,
                WalletId=entity.WalletId
            };
        }

    }
}
