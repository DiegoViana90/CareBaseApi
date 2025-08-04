using Microsoft.AspNetCore.Mvc;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;

namespace CareBaseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientRequestDTO dto)
        {
            try
            {
                Patient created = await _patientService.CreatePatientAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.PatientId }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno ao criar paciente", details = ex.Message });
            }
        }

        // Placeholder para rota de retorno usado no CreatedAtAction
        [HttpGet("{id}")]
        public IActionResult GetById(int id) => NoContent(); // Será implementado depois
    }
}
