using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Mappers
{
    public    class ProfileMapper
    {

         public  Profile ToEntity(UserProfileRequest dto)
        {
            return new Profile {
                DateOfBirth = dto.DateOfBirth,
                Country = dto.Country,
                Phone = dto.Phone,
                UserId = dto.UserId
            };
        }
        public  UserProfileResponse toResponse(Profile entity)
        {
            return new UserProfileResponse
            {
                Id = entity.Id,
                DateOfBirth = entity.DateOfBirth,
                Country = entity.Country,
                Phone = entity.Phone,
                UserId = entity.UserId.ToString()
            };
        }
    }
}
