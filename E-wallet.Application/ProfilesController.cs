using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_wallet.Api.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "User")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost("Create-Profile")]
        
        public async Task<ActionResult> Create([FromBody] UserProfileRequest dto)
        {
            try
            {
                var response = await _profileService.CreateProfileAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("GetProfileById{id}")] 
        public async Task<ActionResult<UserProfileResponse>> GetById(int id)
        {
            var response = await _profileService.GetByIdAsync(id);
            if (response == null)
                return NotFound();
            return Ok(response); 


        }

        [HttpPut("Update-Profile{id}")]
        public async Task<ActionResult<UserProfileResponse>> Update(int id, [FromBody] UserProfileRequest dto)
        {
            try
            {
                var response = await _profileService.UpdateProfileAsync(id, dto);
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
      [HttpGet ("Get-All-Profiles")]
        public async Task<ActionResult<IEnumerable<UserProfileResponse>>> GetAll()
        {
            var profiles = await _profileService.GetAllAsync();
            return Ok(profiles);
        }

        [HttpGet("GetProfileByUserId{UserId}")]
        public async Task<ActionResult<UserProfileResponse>> GetProfileByUserId(int UserId)
        {
            var response = await _profileService.GetByUserIdAsync(UserId);
            if(response==null)
            {
                return NotFound(new { message = "No profile found for this user." });
            }
            return Ok(response);
        }


    } 
}

