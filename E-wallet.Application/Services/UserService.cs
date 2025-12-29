using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Request.Auth;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Mappers;
using E_wallet.Domain.Entities;
using E_wallet.Domain.IHelpers;
using E_wallet.Domain.Interfaces;
using E_wallet.Infrastrucure.Repositories;
namespace E_wallet.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IEmailHelper _emailHelper;
        private readonly IJwtService _jwtService;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ProfileMapper _mapper;
        public UserService(IUserRepository userRepository,
            IEmailHelper mailingHelper,
            IJwtService jwtService,
            ISessionRepository sessionRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _emailHelper = mailingHelper;
            _jwtService = jwtService;
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserRegisterResponse> RegisterUserAsync(UserRegisterRequest dto, CancellationToken ct)
        {
            dto.Email = dto.Email.ToLower();
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email, ct);
            var existingPhone= await _profileRepository.GetByPhoneAsync(dto.Phone, ct);
            if (existingUser != null)
            {
                return UserMapper.Failure("Email is already registered.");
            }
            if (existingPhone != null)
            {
                return UserMapper.Failure("Phone number is already registered.");
            }
            //hashing the password
            dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            //generate OTP 
            var otpCode = new Random().Next(100000, 999999).ToString();
            var otpExpiry = DateTime.UtcNow.AddMinutes(10);
            //generate OTP 
            User user = UserMapper.toEntityRegister(dto);
            user.OtpCode = otpCode;
            user.OtpExpiry = otpExpiry;
            user.IsVerified = false;
            UserProfileRequest profileRequest = new UserProfileRequest
            {
               Phone= dto.Phone,
               DateOfBirth= dto.DateOfBirth,
                UserId = user.Id
            };  
            Profile profile = _mapper.ToEntity(profileRequest);
            //save the user 
            user = await _unitOfWork.Users.AddAsync(user);

           profile= await _unitOfWork.Profiles.AddAsync(profile);


            //create default wallet on register
            Wallet wallet = WalletMapper.ToEntity(new WalletRequest { IsDefault = true, UserId = user.Id });
            _unitOfWork.Wallets.CreateWallet(wallet, ct);
            await _emailHelper.SendOtpEmailAsync(user.Email, user.OtpCode, "User");
            return UserMapper.toResponseRegister(user, profile);
        }

        
        public async Task<UserRegisterResponse> GenaratenewPasswordAsync(NewPasswordrequest dto, CancellationToken ct)
        {
            User user = await _userRepository.GetByIdAsync(dto.id,ct);
            if (user == null)
            {
                return UserMapper.Failure("User does not exist");
            }
            if (user.Password == dto.newPassword)
            {
                return UserMapper.Failure("The new password must be different form the old password");
            }
            user.Password = dto.newPassword;
           
            await _userRepository.UpadteChangesAsync(user, ct);
            return UserMapper.toResponseRegister(user);



            // Send otp code via email
            return UserMapper.toResponseRegister(user);


        }
        public async Task<UserRegisterResponse?> ForgetPasswordAsync(ForgetPasswordEmailrequest dto, CancellationToken ct)
        {

            User user = await _userRepository.GetByEmailAsync(dto.Email, ct);
            if (user == null)
            {
                return null;
            }
            //generate OTP 
            var otpCode = new Random().Next(100000, 999999).ToString();
            var otpExpiry = DateTime.UtcNow.AddMinutes(10);
            //send OPT to email 
            user.OtpCode = otpCode;
            user.OtpExpiry = otpExpiry;
            await _userRepository.UpadteChangesAsync(user, ct);

            await _emailHelper.SendOtpEmailAsync(user.Email, user.OtpCode, user.FullName);

            return UserMapper.toResponseRegister(user);
        }



        public async Task<Result<AuthResponse>> LoginAsync(UserLoginRequest dto, CancellationToken ct)
        {
            //check email if exist 
            User existingUser = await _userRepository.GetByEmailAsync(dto.Email, ct);
            //if (existingUser == null || !string.Equals(dto.Password, existingUser.Password))
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(dto.Password, existingUser.Password))
            {
                return Result<AuthResponse>.Failure("Invalid email or password");
            }
            GenerateTokenRequest generateTokenRequest = AuthMapper.ToTokenRequest(existingUser);
            AuthResponse authResponse = await _jwtService.GenerateToken(generateTokenRequest);

            return Result<AuthResponse>.Success(authResponse);
        }

      public async   Task <string> VerifyOtpAsync(VerifyOtpRequest dto, CancellationToken ct)
        {
            User user = await _userRepository.GetByIdAsync(dto.UserId,ct);
            if (user == null)
                return "User not found";

            if (user.OtpCode != dto.OtpCode)
                return "Invalid OTP";

            if (user.OtpExpiry == null || user.OtpExpiry < DateTime.UtcNow)
                return "OTP expired";

            if (dto.Purpose == "register")
            {
                user.IsVerified = true;
                user.OtpCode = null;
                user.OtpExpiry = null;
                await _userRepository.UpadteChangesAsync(user, ct);
                return "Registration verified successfully";
            }
            else if (dto.Purpose == "resetPassword")
            {
                return "OTP verified successfully, you can reset your password";
            }

            return "Invalid purpose";
        }

        public async Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest refreshRequest)
        {
            Session? storedToken = await _sessionRepository.GetByRefreshTokenAsync(refreshRequest.RefreshToken);
            if (storedToken is null)
                return Result<AuthResponse>.Failure("Invalid or expired refresh token");

            var tokenRequest = AuthMapper.ToTokenRequest(storedToken.User);
            var newAccessToken = _jwtService.GenerateToken(tokenRequest);

            return Result<AuthResponse>.Success(await newAccessToken);
        }

        public async Task<Result> LogoutAsync(RefreshTokenRequest refreshRequest)
        {
            var refreshToken = await _sessionRepository.GetByRefreshTokenAsync(refreshRequest.RefreshToken);

            if (refreshToken != null)
            {
                await _sessionRepository.DeleteAsync(refreshToken.Id);
                return Result.Success();
            }

            return Result.Failure("Refresh token not found");
        }
    }

}