using E_wallet.Domain.Entities;
using E_wallet.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Domain.Interfaces
{
    public interface ILimitRepository
    {
        Task AddAsync(Limit limit);
        Task<Limit> GetLimitsByTypeAndScopeAsync(LimitType type, LimitScope scope);
        Task<Limit> GetLimitByScopeAsync(LimitScope scope);

    }
}
