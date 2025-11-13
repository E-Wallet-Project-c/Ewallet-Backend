using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Interfaces
{
    public interface ILimitService
    {
        Task<Result> CreateLimit(LimitRequest newLimit);
        Task<Limit?> GetLimitsByType(LimitType type, LimitScope scope);
        Task<Limit?> GetLimitByType(LimitType type); 
    }
}
