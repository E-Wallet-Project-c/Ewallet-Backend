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
        public async Task<ActionResult> CreatWallet(WalletRequest NewWallet, CancellationToken ct)
        {
            try
            {
                var response = await _walletService.CreateWallet(NewWallet, ct);



                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");

            }

        }



        [HttpGet("GetUserWallets/{userId}/{PageNumber}/{MaxItem}")]
        public async Task<ActionResult> GetUserWallets([FromRoute] int UserId, [FromRoute] int PageNumber, [FromRoute] int MaxItems, CancellationToken ct)
        {
            try
            {
                var response = await _walletService.GetUserWallets(UserId,PageNumber,MaxItems, ct);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }
        [HttpGet("GetWalletById/{WalletId}")]
        public async Task<ActionResult> GetWalletsById([FromRoute] int WalletId, CancellationToken ct)
        {
            try
            {
                var wallet = await _walletService.GetWalletById(WalletId, ct);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }


        [HttpDelete("DeleteWallet/{WalletId}/{UserId}")]
        public async Task<IActionResult> DeleteWallet([FromRoute] int WalletId ,[FromRoute] int UserId, CancellationToken ct)
        {
            try
            {
                var Wallet = await _walletService.DeleteWalletById(WalletId,UserId, ct);
                return Ok(Wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }

        [HttpDelete("DeleteDefaultWallet/{PrimaryWalletId}/{SecondaryWalletId}/{UserId}")]
        public async Task<IActionResult> DeleteDefaultWallet([FromRoute] int PrimaryWalletId, [FromRoute] int SecondaryWalletId, [FromRoute] int UserId, CancellationToken ct)
        {
            try
            {
                var Wallet = await _walletService.DeleteDefaultWalletById(PrimaryWalletId,SecondaryWalletId,UserId, ct);
                return Ok(Wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }

        [HttpPatch("SetDefaultWallet/{WalletId}/{UserId}")]
        public async Task<IActionResult> SetDefaultWallet([FromRoute] int WalletId , [FromRoute] int UserId, CancellationToken ct)
        {
            try
            {
                var Wallet = await _walletService.SetDefaultWallet(WalletId,UserId, ct);
                return Ok(Wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }

        [HttpGet("GetDefaultWalletByUserId/{userId}")]
        public async Task<ActionResult> GetDefaultWalletByUserId([FromRoute] int userId ,CancellationToken ct)
        {
            try
            {
                var wallet = await _walletService.GetUserDefaultWallet(userId,ct);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }
    }
}
