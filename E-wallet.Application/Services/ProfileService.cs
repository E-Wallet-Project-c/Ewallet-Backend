using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Mappers;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ProfileMapper _mapper;

        public ProfileService(IProfileRepository profileRepository, ProfileMapper profileMapper)
        {
            _profileRepository = profileRepository;

            _mapper = profileMapper;
        }

        public async Task<UserProfileResponse> CreateProfileAsync(UserProfileRequest dto)
        {
            var existing = await _profileRepository.GetByIdAsync(dto.UserId);
            if (existing != null)
                throw new InvalidOperationException("Profile already exists for this user.");


            var profile = _mapper.ToEntity(dto);

            _profileRepository.AddAsync(profile);

            var response = _mapper.toResponse(profile);

            return response;

        }

        public Task<IEnumerable<Profile>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Profile?> GetByIdAsync(int id)
        {//
            var profile = await _profileRepository.GetByIdAsync(id);
            return profile;

        }

        public Task<Profile?> GetByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfileResponse> UpdateProfileAsync(int id, UserProfileRequest dto)
        {
            var profile = await _profileRepository.GetByIdAsync(id);
            if (profile == null)
                throw new KeyNotFoundException("Profile not found.");

            profile.Country = dto.Country;
            profile.Phone = dto.Phone;
            profile.DateOfBirth = dto.DateOfBirth;

            await _profileRepository.UpdateAsync(profile);

            var response = _mapper.toResponse(profile);
            return response;
        }
    }
}