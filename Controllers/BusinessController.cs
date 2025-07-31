using CareBaseApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareBaseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BusinessController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/business
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Business>>> GetBusinesses()
        {
            return await _context.Businesses.Include(b => b.Users).ToListAsync();
        }

        // GET: api/business/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Business>> GetBusiness(int id)
        {
            var business = await _context.Businesses.Include(b => b.Users).FirstOrDefaultAsync(b => b.BusinessId == id);

            if (business == null)
                return NotFound();

            return business;
        }

        // POST: api/business
        [HttpPost]
        public async Task<ActionResult<Business>> CreateBusiness(Business business)
        {
            _context.Businesses.Add(business);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBusiness), new { id = business.BusinessId }, business);
        }

        // PUT: api/business/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBusiness(int id, Business updatedBusiness)
        {
            if (id != updatedBusiness.BusinessId)
                return BadRequest();

            _context.Entry(updatedBusiness).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/business/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusiness(int id)
        {
            var business = await _context.Businesses.FindAsync(id);
            if (business == null)
                return NotFound();

            _context.Businesses.Remove(business);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BusinessExists(int id)
        {
            return _context.Businesses.Any(b => b.BusinessId == id);
        }
    }
}
