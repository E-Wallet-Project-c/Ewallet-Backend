using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
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
        public async Task<ActionResult> CreatWallet (WalletRequest NewWallet)
        {
            try
            {
                var response = await _walletService.CreateWallet(NewWallet);

                if (!response.IsSuccess)
                {
                    return BadRequest(response.ErrorMessage);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
                
            }
            
        }

        [HttpGet("GetUserWallets{UserId}")]
        public async Task<ActionResult> GetUserWallets(WalletRequest NewWallet)
        {
            try
            {
                var response = await _walletService.GetUserWallets(NewWallet);
                if (!response.IsSuccess)
                {
                    return NotFound( response.ErrorMessage );
                }
                return Ok(response);
            }
            catch (Exception ex) {
                return StatusCode(500, $"{ex.Message}");
            }
           
        }
    }
}
