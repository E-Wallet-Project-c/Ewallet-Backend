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
        private readonly IUnitOfWork _unitOfWork; 

        public LimitService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> CreateLimit(LimitRequest NewLimit) 
        {
            if (NewLimit == null)
            {
                return Result.Failure("Limit data cannot be null.");
            }

            var limit = LimitMapper.ToEntity(NewLimit);

            await _unitOfWork.Limits.AddAsync(limit);

            await _unitOfWork.CompleteAsync();

            return Result.Success(); 
        }


        public async Task<Result<Limit>> GetLimitByScope(LimitScope scope) 
        {
            var limit = await _unitOfWork.Limits.GetLimitByScopeAsync(scope);

            if (limit == null)
            {
                return null; 
            }

            return Result<Limit>.Success(limit); 
        }

        public Task<Limit?> GetLimitByType(LimitType type)
        {
            throw new NotImplementedException();
        }

        public async Task<Limit> GetLimitsByType(LimitType type, LimitScope scope)
        {
            var limit = await _unitOfWork.Limits.GetLimitsByTypeAndScopeAsync(type, scope);
            return limit;
        }

    }
}