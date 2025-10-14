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
        Task<UserRegisterResponse> RegisterUserAsync(UserRegisterRequest dto);

        Task<UserRegisterResponse> ForgetPasswordAsync(ForgetPasswordEmailrequest dto);
        Task<UserRegisterResponse> GenaratenewPasswordAsync(NewPasswordrequest dto);

        Task<UserLoginResponse> LoginUserAsync(UserLoginRequest dto);

    }
}
