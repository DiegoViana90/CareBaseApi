using CareBaseApi.Models;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using CareBaseApi.Utils;

namespace CareBaseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;


        public BusinessController(IBusinessService businessService, IConfiguration configuration, IUserService userService)
        {
            _businessService = businessService;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Business>>> GetBusinesses()
        {
            var businesses = await _businessService.GetAllAsync();
            return Ok(businesses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Business>> GetBusiness(int id)
        {
            var business = await _businessService.GetByIdAsync(id);
            if (business == null)
                return NotFound();

            return Ok(business);
        }

        [HttpPost("create-business")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateBusiness(CreateBusinessRequestDTO createBusinessRequestDTO)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            var jwtSecret = _configuration["Jwt:Secret"];
            var userRole = TokenUtils.GetRoleFromToken(token, jwtSecret);

            var createBusinessDTO = new CreateBusinessDTO
            {
                BusinessName = createBusinessRequestDTO.BusinessName,
                BusinessEmail = createBusinessRequestDTO.BusinessEmail,
                BusinessTaxNumber = createBusinessRequestDTO.BusinessTaxNumber,
                Role = userRole // será null se token inválido ou ausente
            };

            var createdBusiness = await _businessService.CreateBusinessAsync(createBusinessDTO);

            return CreatedAtAction(nameof(GetBusiness), new { id = createdBusiness.BusinessId }, new
            {
                message = "Empresa criada com sucesso",
                data = new
                {
                    createdBusiness.BusinessId,
                    createdBusiness.Name,
                    createdBusiness.Email,
                    createdBusiness.TaxNumber,
                    createdBusiness.ExpirationDate
                }
            });
        }

        [HttpPost("test-account")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateTestAccount([FromBody] CreateTestAccountRequestDTO dto)
        {
            try
            {
                var createdBusiness = await _businessService.CreateTestAccountAsync(dto);

                return Created("", new
                {
                    message = "Conta teste criada com sucesso",
                    businessId = createdBusiness.BusinessId
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = $"Erro: {e.Message}" });
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBusiness(int id, Business updatedBusiness)
        {
            if (id != updatedBusiness.BusinessId)
                return BadRequest();

            try
            {
                await _businessService.UpdateAsync(updatedBusiness);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusiness(int id)
        {
            try
            {
                await _businessService.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
