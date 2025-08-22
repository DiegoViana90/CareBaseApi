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
        public async Task<IActionResult> GetConsultationsByPatient(int patientId)
        {
            try
            {
                var consultations = await _consultationService.GetConsultationsByPatientWithNameAsync(patientId);

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

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetDetails(int id)
        {
            try
            {
                var details = await _consultationService.GetDetailsFullAsync(id);
                if (details == null)
                    return NotFound(new { message = "Detalhes não encontrados." });

                return Ok(new { message = "Detalhes encontrados", data = details });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao buscar detalhes", details = ex.Message });
            }
        }


        [HttpPut("{id}/details")]
        [SwaggerOperation(Summary = "Atualizar ou criar detalhes da consulta")]
        public async Task<IActionResult> AddOrUpdateDetails(int id, [FromBody] UpdateConsultationDetailsRequestDTO dto)
        {
            try
            {
                if (id != dto.ConsultationId)
                    return BadRequest(new { message = "ID da URL não corresponde ao corpo da requisição." });

                await _consultationService.AddOrUpdateConsultationDetailsAsync(dto);

                return Ok(new { message = "Detalhes salvos com sucesso." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao salvar detalhes", details = ex.Message });
            }
        }


        [HttpGet("{id}/payments")]
        [SwaggerOperation(Summary = "Listar pagamentos de uma consulta")]
        public async Task<IActionResult> GetPayments(int id)
        {
            try
            {
                var list = await _consultationService.GetPaymentsAsync(id);
                return Ok(new { message = "Pagamentos encontrados", data = list });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao buscar pagamentos", details = ex.Message });
            }
        }

        [HttpPost("{id}/payments")]
        [SwaggerOperation(Summary = "Criar pagamentos (1..N) para a consulta")]
        public async Task<IActionResult> CreatePayments(int id, [FromBody] CreatePaymentsRequestDTO dto)
        {
            try
            {
                if (id != dto.ConsultationId)
                    return BadRequest(new { message = "ID da URL não corresponde ao corpo da requisição." });

                var created = await _consultationService.AddPaymentsAsync(id, dto.Lines);

                return Created("", new
                {
                    message = "Pagamentos criados com sucesso",
                    data = created
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Erro ao criar pagamentos",
                    details = ex.Message
                });
            }
        }

    }
}