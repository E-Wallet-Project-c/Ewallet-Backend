using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Interfaces;

using E_wallet.Application.Services;
using E_wallet.Domain.Entities;
using E_wallet.Domain.IHelpers;
using E_wallet.Infrastrucure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML.Voice;

namespace E_wallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       

       

        private readonly IUserService _userService;
        private readonly ISMSHelper _sms;

        private readonly IEmailHelper _IHelper;
        

        public AuthController(IUserService userService, ISMSHelper sms, IEmailHelper IHelper)
        {
            _userService = userService;
            _sms = sms;
            _IHelper = IHelper;
        
        }

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] UserRegisterRequest request)
        //{

        //}


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest registerDto, CancellationToken ct)
        {
            try
            {
                var result = await _userService.RegisterUserAsync(registerDto,ct);

                if (!result.Success)
                    return BadRequest(new { message = result.Message });

                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshToken)
        {
            var result = await _userService.RefreshTokenAsync(refreshToken);
            try
            {
                if (!result.IsSuccess)
                    return BadRequest(new { message = result.ErrorMessage });
               
                if (!IsMobileClient())
                {
                    SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.Expiries);
                    return Ok(result);
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest loginDto, CancellationToken ct)
        {
            try
            {
                var result = await _userService.LoginAsync(loginDto,ct);

                if (!result.IsSuccess)
                    return BadRequest(new { message = result.ErrorMessage });


                if (!IsMobileClient())
                {
                    SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.Expiries);
                    return Ok(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest dto)
        {
            try
            {
                var result = await _userService.LogoutAsync(dto);
                if (!result.IsSuccess)
                    return BadRequest(new { message = result.ErrorMessage });
                return Ok(new { message = "Logout successful" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("ForgetPassword/{Email}")]
        public async Task<IActionResult> Checkemail([FromRoute] ForgetPasswordEmailrequest dto, CancellationToken ct)
        {
          
            var v=await _userService.ForgetPasswordAsync(dto, ct);
            if(v==null)
                return BadRequest(new { message = "Email not found" });
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

      

        private void SetRefreshTokenCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Ensure this is true in production
                SameSite = SameSiteMode.Strict,
                Expires = expires
            };
            Response.Cookies.Append("X-Refresh-Token", refreshToken, cookieOptions);
        }

        [HttpPost("SendSMSmessage")]
        public async Task<IActionResult> SendSMSmessage()
        {
            await _sms.SendSmsAsync("+962 7 9989 5351 ","Hello");
            return Ok();
        }


        private bool IsMobileClient() 
        {
            return Request.Headers.TryGetValue("X-Client-Type", out var clientType) && clientType == "mobile";
        }
    } 
}
