using E_wallet.Application.Interfaces;
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
    }
}
