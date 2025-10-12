using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Mappers;
using E_wallet.Application.Validators;
using E_wallet.Domain.Entities;
using E_wallet.Domain.IHelpers;
using E_wallet.Domain.Interfaces;
using E_wallet.Infrastrucure.Helpers;
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
        private readonly IEmailHelper _emailHelper;
        public UserService(IUserRepository userRepository, IEmailHelper mailingHelper)
        {
            _userRepository = userRepository;
            _emailHelper = mailingHelper;
        }

        public async Task<UserRegisterResponse> RegisterUserAsync(UserRegisterRequest dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return UserMapper.Failure("Email is already registered.");
            }
            //generate OTP 
            var otpCode = new Random().Next(100000, 999999).ToString();
            var otpExpiry = DateTime.UtcNow.AddMinutes(10);
            //generate OTP 
            User user = UserMapper.toEntityRegister(dto);
            user.OtpCode = otpCode;
            user.OtpExpiry = otpExpiry;
            user.IsVerified = false;

            user = await _userRepository.AddAsync(user);

            await _emailHelper.SendOtpEmailAsync(user.Email, user.OtpCode);

            return UserMapper.toResponseRegister(user);

        }
        public async Task<UserRegisterResponse> ForgetPasswordAsync(ForgetPasswordEmailrequest dto)
        {

            User user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user==null)
            {
                return UserMapper.Failure("User does not exist");
            }
            //generate OTP 
            var otpCode = new Random().Next(100000, 999999).ToString();
            var otpExpiry = DateTime.UtcNow.AddMinutes(10);
            //send OPT to email 
            user.OtpCode = otpCode;
            user.OtpExpiry = otpExpiry;
            await _userRepository.UpadteChangesAsync(user);

            await _emailHelper.SendOtpEmailAsync(user.Email, user.OtpCode);

           return UserMapper.toResponseRegister(user);
        }

        
        public async Task<UserRegisterResponse> GenaratenewPasswordAsync(NewPasswordrequest dto)
        {
            User user = await _userRepository.GetByIdAsync(dto.id);
            if (user == null)
            {
                return UserMapper.Failure("User does not exist");
            }
            if (user.Password==dto.newPassword)
            {
                return UserMapper.Failure("The new password must be different form the old password");
            }
            user.Password = dto.newPassword;
           
            await _userRepository.UpadteChangesAsync(user);
            return UserMapper.toResponseRegister(user);
        }


    }

}
