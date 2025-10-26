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
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("{walletId}/balance")]
        public async Task<IActionResult> GetBalance(int walletId)
        {
            var balanceDto = await _walletService.GetWalletBalanceAsync(walletId);

            if (balanceDto == null)
                return NotFound(new { message = $"Wallet with ID {walletId} not found." });

            return Ok(balanceDto);
        }

        [HttpPost("CreatWallet{UserId}")]
        public async Task<ActionResult<WalletResponse>> CreatWallet (int UserId)
        {
            var response = await _walletService.CreateWallet(UserId);

            if (response.WalletId == 0)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("GetUserWallets{UserId}")]
        public async Task<ActionResult<List<WalletResponse>>> GetUserWallets(int UserId)
        {
            var response = await _walletService.GetUserWallets(UserId);
           if (response.Count == 0)
            {
                return NotFound(new { message = "No wallets found for this user." });
            }
            return Ok(response);
        }


        [HttpPost("TopUpWallet")]
        public async Task<IActionResult> TopUpWallet([FromBody] TopUpWithdrawRequest request)
        {
            var response = await _walletService.TopUpToWalletAsync(request);

            if (!response.IsSuccess)
                return BadRequest(response);


            return Ok(response);
        }

        [HttpPost("WithdrawWallet")]
        public async Task<IActionResult> WithdrawBankAccount([FromBody] TopUpWithdrawRequest request)
        {
            var response = await _walletService.WithdrawFromWalletAsync(request);

            if (!response.IsSuccess)
                return BadRequest(response);


            return Ok(response);
        }
    }
}
