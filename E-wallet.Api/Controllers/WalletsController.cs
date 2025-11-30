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



        [HttpGet("GetUserWallets/{UserId}")]
        public async Task<ActionResult> GetUserWallets([FromRoute] int UserId, CancellationToken ct)
        {
            try
            {
                var response = await _walletService.GetUserWallets(UserId, ct);

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


        [HttpPost("TopUpWallet")]
        public async Task<IActionResult> TopUpWallet([FromBody] TopUpWithdrawRequest request, CancellationToken ct)
        {
            var response = await _walletService.TopUpToWalletAsync(request,ct);

            if (!response.IsSuccess)
                return BadRequest(response);


            return Ok(response);
        }

        [HttpPost("WithdrawWallet")]
        public async Task<IActionResult> WithdrawBankAccount([FromBody] TopUpWithdrawRequest request, CancellationToken ct)
        {
            var response = await _walletService.WithdrawFromWalletAsync(request,ct);

            if (!response.IsSuccess)
                return BadRequest(response);


            return Ok(response);
        }

        [HttpPost("TransferFromWallet")]
        public async Task<IActionResult> TransferFromWallet([FromBody] TransferRequest request, CancellationToken ct)
        {
            var response = await _walletService.TransferFromWalletAsync(request,ct);

            if (!response.IsSuccess)
                return BadRequest(response);


            return Ok(response);
        }

        [HttpDelete("DeleteWallet")]
        public async Task<IActionResult> DeleteWallet([FromBody] WalletRequest wallet, CancellationToken ct)
        {
            try
            {
                var Wallet = await _walletService.DeleteWalletById(wallet, ct);
                return Ok(Wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }

        [HttpDelete("DeleteDefaultWallet")]
        public async Task<IActionResult> DeleteDefaultWallet([FromBody] DefaultWalletDeleteRequest wallet, CancellationToken ct)
        {
            try
            {
                var Wallet = await _walletService.DeleteDefaultWalletById(wallet, ct);
                return Ok(Wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }

        [HttpPatch("SetDefaultWallet")]
        public async Task<IActionResult> SetDefaultWallet([FromBody] WalletRequest wallet, CancellationToken ct)
        {
            try
            {
                var Wallet = await _walletService.SetDefaultWallet(wallet, ct);
                return Ok(Wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }
    }
}
