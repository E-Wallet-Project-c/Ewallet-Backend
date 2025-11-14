using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Interfaces;
using E_wallet.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_wallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiaryController : ControllerBase
    {
        private readonly IBeneficiaryService _beneficiaryService;
        public BeneficiaryController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;

        }
        [HttpPost("Create-Beneficiary")]
        public async Task<IActionResult> CreateBeneficiary(BeneficiaryRequest beneficiary)
        {
            try
            {

                var response = await _beneficiaryService.CreateBeneficiary(beneficiary);
                if (!response.IsSuccess)
                {
                    return BadRequest(new { message = response.ErrorMessage });
                }
                return Ok(response);



            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("GET-Beneficiary-By-Id{id}")]
        public async Task<IActionResult> GetBeneficiaryBtId([FromRoute] int id)
        {
            try
            {
                var response = await _beneficiaryService.GetBeneficiaryById(id);
                if (!response.IsSuccess)
                {
                    return BadRequest(new { message = response.ErrorMessage });
                }
                return Ok(response);


            }

            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });

            }
        }
            [HttpGet("GetBeneficiariesByWalletId/{walletId}")]
            public async Task<IActionResult> GetBeneficiariesByWalletId(int walletId)
            {
                var result = await _beneficiaryService.GetAllBeneficiariesByWalletId(walletId);

                if (!result.IsSuccess)
                {
                    return NotFound(new
                    {
                        message = result.ErrorMessage
                    });
                }

                return Ok(result);
            }
        }
    } 