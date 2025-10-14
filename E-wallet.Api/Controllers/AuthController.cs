using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Services;
using E_wallet.Domain.Entities;
using E_wallet.Infrastrucure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
            try
            {
                var result = await _userService.RegisterUserAsync(registerDto);

                if (!result.Success)
                    return BadRequest(new { message = result.Message });

                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest loginDto)
        {
            try
            {
                var result = await _userService.LoginUserAsync(loginDto);

                if (!result.Success)
                    return BadRequest(new { message = result.Message });

                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> Checkemail([FromBody] ForgetPasswordEmailrequest dto)
        {

            await _userService.ForgetPasswordAsync(dto);
            return Ok(new { message = "OTP sent to your email" });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> Reset([FromBody] NewPasswordrequest dto)
        {
            var result = await _userService.GenaratenewPasswordAsync(dto);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = "Password has been reset successfully" });
            try
            {
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
            [HttpPost("verify-otp")]
            public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest dto)
            {
                var result = await _userService.VerifyOtpAsync(dto);
                if (result.Contains("Invalid") || result.Contains("expired") || result.Contains("not found"))
                    return BadRequest(new { message = result });

                return Ok(new { message = result });
            }
        
    } 
}
