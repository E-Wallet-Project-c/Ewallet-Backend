using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Mappers;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Enums;
using E_wallet.Domain.Interfaces;

namespace E_wallet.Application.Services
{
    public class LimitService : ILimitService
    {
        private readonly ILimitRepository _limitRepository;

        public LimitService(ILimitRepository limitRepository)
        {
            _limitRepository = limitRepository;
        }
        public async Task CreateLimit(LimitRequest NewLimit)
        {
            var limit = LimitMapper.ToEntity(NewLimit);
            await _limitRepository.AddAsync(limit);
             Result.Success();
   
        }

        public Task<Limit> GetLimitByScope(LimitScope scope)
        {
            var limit = _limitRepository.GetLimitByScopeAsync(scope);
            return limit;
        }

        public Task<Limit> GetLimitsByType(LimitType type, LimitScope scope)
        {
            var limit = _limitRepository.GetLimitsByTypeAndScopeAsync(type, scope);
            return limit;
        }

        public Task<Limit> GetLimitsByType(LimitType type)
        {
            throw new NotImplementedException();
        }
    }
}
