using CareBaseApi.Models;
using CareBaseApi.Services.Interfaces;// Namespace do service
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CareBaseApi.Dtos.Requests;
using Swashbuckle.AspNetCore.Annotations;

namespace CareBaseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(
       Summary = "Cria um novo usuário vinculado a um CNPJ",
       Description = "O CPF (TaxNumber) do usuário e o CNPJ da empresa devem ser válidos."
   )]
        public async Task<IActionResult> CreateUser(CreateUserRequestDTO createUserRequestDTO)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(createUserRequestDTO);

                return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, new
                {
                    message = "Usuário criado com sucesso",
                    data = new
                    {
                        createdUser.UserId,
                        createdUser.Name, // ✅ Adicionado
                        createdUser.Email,
                        createdUser.TaxNumber,
                        Role = createdUser.Role.ToString(),
                        createdUser.BusinessId
                    }
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // PUT: api/user/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            if (id != updatedUser.UserId)
                return BadRequest();

            try
            {
                await _userService.UpdateUserAsync(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
