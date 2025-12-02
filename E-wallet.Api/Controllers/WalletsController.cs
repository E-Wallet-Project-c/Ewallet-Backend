using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Services;
using E_wallet.Domain.Entities;
using E_wallet.Infrastrucure.Repositories;
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
        public async Task<ActionResult> CreatWallet(WalletRequest NewWallet)
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
        public async Task<ActionResult> GetUserWallets([FromRoute] int UserId)
        {
            try
            {
                var response = await _walletService.GetUserWallets(UserId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }
        [HttpGet("GetWalletById/{WalletId}")]
        public async Task<ActionResult> GetWalletsById([FromRoute] int WalletId)
        {
            try
            {
                var wallet = await _walletService.GetWalletById(WalletId);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }


       
        [HttpDelete("DeleteWallet")]
        public async Task<IActionResult> DeleteWallet([FromBody] WalletRequest wallet)
        {
            try
            {
                var Wallet = await _walletService.DeleteWalletById(wallet);
                return Ok(Wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }

        [HttpDelete("DeleteDefaultWallet")]
        public async Task<IActionResult> DeleteDefaultWallet([FromBody] DefaultWalletDeleteRequest wallet)
        {
            try
            {
                var Wallet = await _walletService.DeleteDefaultWalletById(wallet);
                return Ok(Wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }

        [HttpPatch("SetDefaultWallet")]
        public async Task<IActionResult> SetDefaultWallet([FromBody] WalletRequest wallet)
        {
            try
            {
                var Wallet = await _walletService.SetDefaultWallet(wallet);
                return Ok(Wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }

        [HttpGet("GetDefaultWalletByUserId/{userId}")]
        public async Task<ActionResult> GetDefaultWalletByUserId([FromRoute] int userId)
        {
            try
            {
                var wallet = await _walletService.GetUserDefaultWallet(userId);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }
    }
}
