using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_wallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public TransactionsController(IWalletService walletService)
        {
            _walletService = walletService;
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
