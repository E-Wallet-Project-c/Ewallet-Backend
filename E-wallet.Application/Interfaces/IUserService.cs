using E_wallet.Application.Dtos.Request;
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
        Task<UserLoginResponse> LoginUserAsync(UserLoginRequest dto);

    }
}
