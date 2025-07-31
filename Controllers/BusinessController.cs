using CareBaseApi.Models;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Dtos.Requests;
using CareBaseApi.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareBaseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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

        [HttpPost]
        public async Task<IActionResult> CreateBusiness(CreateBusinessRequestDTO createBusinessRequestDTO)
        {

            var business = new Business
            {
                Name = createBusinessRequestDTO.BusinessName,
                TaxNumber = createBusinessRequestDTO.BusinessTaxNumber,
                Email = createBusinessRequestDTO.BusinessEmail
            };

            var createdBusiness = await _businessService.CreateAsync(business);

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
