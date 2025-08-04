using CareBaseApi.Dtos.Requests;
using CareBaseApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CareBaseApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationsController : ControllerBase
    {
        private readonly IConsultationService _consultationService;

        public ConsultationsController(IConsultationService consultationService)
        {
            _consultationService = consultationService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Criar nova consulta")]
        public async Task<IActionResult> CreateConsultation([FromBody] CreateConsultationRequestDTO dto)
        {
            try
            {
                var created = await _consultationService.CreateConsultationAsync(dto);

                return Created("", new
                {
                    message = "Consulta criada com sucesso",
                    data = created
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno ao criar consulta", details = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}")]
        [SwaggerOperation(Summary = "Listar consultas de um paciente")]
        public async Task<IActionResult> GetConsultationsByPatient(int patientId)
        {
            try
            {
                var consultations = await _consultationService.GetConsultationsByPatientAsync(patientId);

                return Ok(new
                {
                    message = "Consultas encontradas",
                    data = consultations
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao buscar consultas", details = ex.Message });
            }
        }
    }
}
