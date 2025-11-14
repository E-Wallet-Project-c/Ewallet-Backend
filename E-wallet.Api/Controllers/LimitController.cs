using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_wallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LimitController : ControllerBase
    {
        private readonly ILimitService _limitService;
        public LimitController(ILimitService limitService) 
        {
            _limitService = limitService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(LimitRequest request)
        {
            try
            {
                await _limitService.CreateLimit(request);
                return Ok();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
