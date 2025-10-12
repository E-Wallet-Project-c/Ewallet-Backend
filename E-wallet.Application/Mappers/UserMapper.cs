using E_wallet.Application.Dtos.Request;
using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Mappers
{
    public static class UserMapper
    {
        public static User toEntityRegister(UserRegisterRequest dto )
        {
            return new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,
                IsActive = true,



            };
        }

        public static UserRegisterResponse toResponseRegister(User entity)
        {
            return new UserRegisterResponse
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Email = entity.Email,
                //IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt

                
            };
        }

        public static UserRegisterResponse Failure(string message)
        {
            return new UserRegisterResponse
            {
                Success = false,
                Message = message
            };
        }

        public static UserLoginResponse toResponseLogin(User entity)
        {
            return new UserLoginResponse
            {
                Id = entity.Id,
            };
        }

        public static UserLoginResponse FailureLogin(string message)
        {
            return new UserLoginResponse
            {
                Success = false,
                Message = message
            };
        }
    }
}
