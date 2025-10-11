using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Services;
using E_wallet.Domain.Entities;
using E_wallet.Infrastrucure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_wallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] UserRegisterRequest request)
        //{

        //}

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest registerDto)
        {
            var result = await _userService.RegisterUserAsync(registerDto);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }
    }
}
