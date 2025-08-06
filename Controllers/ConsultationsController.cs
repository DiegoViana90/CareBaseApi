using Microsoft.AspNetCore.Mvc;
using CareBaseApi.Services.Interfaces;
using CareBaseApi.Dtos.Requests;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

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

        private int GetBusinessIdFromToken()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "BusinessId");
            if (claim == null)
                throw new UnauthorizedAccessException("BusinessId não encontrado no token.");
            return int.Parse(claim.Value);
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

        [HttpGet]
        [SwaggerOperation(Summary = "Listar todas as consultas da empresa logada")]
        public async Task<IActionResult> GetAllConsultations()
        {
            try
            {
                var businessId = GetBusinessIdFromToken();
                var consultations = await _consultationService.GetAllConsultationsByBuAsync(businessId);

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
