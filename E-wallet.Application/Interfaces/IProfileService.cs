using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Interfaces
{
    public interface  IProfileService
    {
        Task<UserProfileResponse> CreateProfileAsync(UserProfileRequest dto);//done


        Task<Profile?> GetByIdAsync(int id);// done

        Task<Profile?> GetByUserIdAsync(int userId);

        Task<UserProfileResponse> UpdateProfileAsync(int id, UserProfileRequest dto);//done 

        Task<IEnumerable<UserProfileResponse>> GetAllAsync();// done
    }
}
