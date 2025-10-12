using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Mappers;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserRegisterResponse> RegisterUserAsync(UserRegisterRequest dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return UserMapper.Failure("Email is already registered.");
            }
            //hashing the password
            //dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            //generate OTP 
            var otpCode = new Random().Next(100000, 999999).ToString();
            var otpExpiry = DateTime.UtcNow.AddMinutes(10);
            //generate OTP 
            User user = UserMapper.toEntityRegister(dto);
            user.OtpCode = otpCode;
            user.OtpExpiry = otpExpiry;
            user.IsVerified = false;

            user = await _userRepository.AddAsync(user);

            // Send otp code via email
            return UserMapper.toResponseRegister(user);

            
        }

        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest dto)
        {
            //check email if exist 
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            //!BCrypt.Net.BCrypt.Verify(dto.Password, existingUser.Password) 
            if (existingUser == null || !string.Equals(dto.Password, existingUser.Password))
            {
                return UserMapper.FailureLogin("Invalid email or password");
            }

            return UserMapper.toResponseLogin(existingUser);
        }

            
    }

}
