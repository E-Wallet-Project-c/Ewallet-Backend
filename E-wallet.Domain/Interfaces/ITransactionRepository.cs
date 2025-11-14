using E_wallet.Domain.Entities;
using EllipticCurve.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddAsync(Transaction transaction);
        int GetAllByDay(int walletId, DateTime date);

        decimal GetTotalAmountByDay(int walletId, DateTime date);
    }
}
