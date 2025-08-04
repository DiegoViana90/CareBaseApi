using Microsoft.AspNetCore.Mvc;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Dtos.Requests;
using CareBaseApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace CareBaseApi.Controllers
{
    [Authorize]
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
        [SwaggerOperation(Summary = "Criar paciente")]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientRequestDTO dto)
        {
            try
            {
                // Recupera o BusinessId do token JWT
                var businessIdClaim = User.Claims.FirstOrDefault(c => c.Type == "BusinessId");
                if (businessIdClaim == null)
                    return Unauthorized(new { message = "BusinessId não encontrado no token." });

                var businessId = int.Parse(businessIdClaim.Value);

                var created = await _patientService.CreatePatientAsync(dto, businessId);

                return Created("", new
                {
                    message = "Paciente criado com sucesso",
                    data = created
                });
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
        [HttpGet]
        [SwaggerOperation(Summary = "Listar pacientes da empresa logada")]
        public async Task<IActionResult> GetAllPatients()
        {
            try
            {
                var businessIdClaim = User.Claims.FirstOrDefault(c => c.Type == "BusinessId");
                if (businessIdClaim == null)
                    return Unauthorized(new { message = "BusinessId não encontrado no token." });

                var businessId = int.Parse(businessIdClaim.Value);
                var patients = await _patientService.GetPatientsByBusinessAsync(businessId);

                return Ok(new
                {
                    message = "Pacientes encontrados",
                    data = patients
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao buscar pacientes", details = ex.Message });
            }
        }


    }
}
