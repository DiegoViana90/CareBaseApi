using CareBaseApi.Models;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Dtos.Requests;
using CareBaseApi.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

namespace CareBaseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
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

        [HttpPost("CreateBusiness")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Cria um novo negócio (Business)", Description = "Cria um novo registro de business no sistema.")]
        public async Task<IActionResult> CreateBusiness(CreateBusinessRequestDTO request)
        {
            string? userRole = null;

            try
            {
                var user = HttpContext.User;
                if (user?.Identity?.IsAuthenticated == true)
                {
                    userRole = user.FindFirst(ClaimTypes.Role)?.Value;
                }
            }
            catch
            {
                return Unauthorized(new { message = "Token inválido ou malformado." });
            }

            CreateBusinessDTO createBusinessDTO = new CreateBusinessDTO
            {
                BusinessName = request.BusinessName,
                BusinessEmail = request.BusinessEmail,
                BusinessTaxNumber = request.BusinessTaxNumber,
                Role = userRole
            };

            var createdBusiness = await _businessService.CreateBusinessAsync(createBusinessDTO);

            var response = new CreateBusinessResponseDTO
            {
                BusinessId = createdBusiness.BusinessId,
                BusinessName = createdBusiness.Name,
                TaxNumber = createdBusiness.TaxNumber,
                Email = createdBusiness.Email
            };

            return CreatedAtAction(nameof(GetBusiness), new { id = createdBusiness.BusinessId }, new
            {
                message = "Business created successfully",
                data = response
            });
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
