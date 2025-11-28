using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Services;
using E_wallet.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML.Voice;

namespace E_wallet.Api.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "User")]
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

        [HttpPost("CreatWallet")]
        public async Task<ActionResult> CreatWallet ( WalletRequest NewWallet)
        {
            try
            {
                var response = await _walletService.CreateWallet(NewWallet);

              

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
                
            }
            
        }



        [HttpGet("GetUserWallets/{UserId}")]
        public async Task<ActionResult> GetUserWallets( [FromRoute]int UserId)
        {
            try
            {
                var response = await _walletService.GetUserWallets(UserId);
            
                return Ok(response);
            }
            catch (Exception ex) {
                return StatusCode(500, $"{ex.Message}");
            }
           
        }
        [HttpGet("GetWalletById/{WalletId}")]
        public async Task<ActionResult> GetWalletsById([FromRoute]int WalletId)
        {
            try
            {
                var wallet= await _walletService.GetWalletById(WalletId);
                return Ok(wallet);
            }
            catch (Exception ex) {
                return StatusCode(500, $"{ex.Message}");
            }
           
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

        [HttpPost("TransferFromWallet")]
        public async Task<IActionResult> TransferFromWallet([FromBody] TransferRequest request)
        {
            var response = await _walletService.TransferFromWalletAsync(request);

            if (!response.IsSuccess)
                return BadRequest(response);


            return Ok(response);
        }

    }
}
