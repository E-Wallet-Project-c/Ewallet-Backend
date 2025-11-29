using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserRegisterResponse> RegisterUserAsync(UserRegisterRequest dto, CancellationToken ct);

        Task<UserRegisterResponse> ForgetPasswordAsync(ForgetPasswordEmailrequest dto, CancellationToken ct);
        Task<UserRegisterResponse> GenaratenewPasswordAsync(NewPasswordrequest dto);

        Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest refreshRequest);
        Task<Result<AuthResponse>> LoginAsync(UserLoginRequest dto, CancellationToken ct);
        Task<Result> LogoutAsync(RefreshTokenRequest refreshRequest);

        Task<string> VerifyOtpAsync(VerifyOtpRequest dto);


    }
}
