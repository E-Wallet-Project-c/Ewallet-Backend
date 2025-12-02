using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Services;
using E_wallet.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGrid;

namespace E_wallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBankAccountsController : ControllerBase
    {
        private readonly IUserBankAccountService _userBankAccountService;

        public UserBankAccountsController(IUserBankAccountService userBankAccountService)
        {
            _userBankAccountService = userBankAccountService;
        }


        [HttpPost("Create-User-Bank-Account")]
        public async Task<IActionResult> CreateUserBankAccount([FromBody] UserBankAccountRequest request)
        {
            var response = await _userBankAccountService.CreateBankAsync(request);

            if (!response.IsSuccess)
                return BadRequest(response);
            

            return Ok(response);
        }

        [HttpGet("Get-All-User-Bank-Account/{walletId}")]
        public async Task<IActionResult> GetAllByWalletIdAsync(int walletId)
        {
            var response = await _userBankAccountService.GetAllByWalletIdAsync(walletId);
            if (!response.IsSuccess)
                return BadRequest(response);


            return Ok(response);
        }


        [HttpPost("Update-Status-User-Bank-Account")]
        public async Task<IActionResult> GetAllByWalletIdAsync(UpdateUserBankAccountRequest dto )
        {
            var response = await _userBankAccountService.UpdateStatusAsync(dto);
            if (!response.IsSuccess)
                return BadRequest(response);


            return Ok(response);
        }


    }
}
