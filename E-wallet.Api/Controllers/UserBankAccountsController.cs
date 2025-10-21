using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Services;
using E_wallet.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


        [HttpPost]
        public async Task<IActionResult> CreateUserBankAccount([FromBody] UserBankAccountRequest request)
        {
            var response = await _userBankAccountService.CreateBankAsync(request);

            if (!response.IsSuccess)
                return BadRequest(response);
            

            return Ok(response);
        }

    }
}
