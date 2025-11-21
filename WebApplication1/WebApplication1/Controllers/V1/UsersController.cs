using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(AppDbContext context, ILogger<UsersController> logger)
        {
     _context = context;
     _logger = logger;
      }

        /// <summary>
 /// Obtém todos os usuários com paginação
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<UserDto>>> GetUsers(
   [FromQuery] int pageNumber = 1,
      [FromQuery] int pageSize = 10)
        {
_logger.LogInformation("Buscando usuários - Página: {PageNumber}", pageNumber);

   var totalCount = await _context.Users.CountAsync();
   var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

    var users = await _context.Users
         .OrderByDescending(u => u.CreatedAt)
   .Skip((pageNumber - 1) * pageSize)
       .Take(pageSize)
.ToListAsync();

       var userDtos = users.Select(u => MapToDto(u)).ToList();

  return Ok(new PagedResult<UserDto>
 {
      Items = userDtos,
     PageNumber = pageNumber,
            PageSize = pageSize,
  TotalPages = totalPages,
      TotalCount = totalCount
            });
     }

     /// <summary>
        /// Obtém um usuário específico por ID
  /// </summary>
 [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            _logger.LogInformation("Buscando usuário com ID: {Id}", id);

   var user = await _context.Users.FindAsync(id);

         if (user == null)
 {
    _logger.LogWarning("Usuário com ID {Id} não encontrado", id);
   return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
     }

            return Ok(MapToDto(user));
 }

        /// <summary>
        /// Cria um novo usuário
      /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
   public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createDto)
    {
     _logger.LogInformation("Criando novo usuário: {Email}", createDto.Email);

            if (!ModelState.IsValid)
            {
       return BadRequest(ModelState);
    }

       // Verificar se email já existe
       if (await _context.Users.AnyAsync(u => u.Email == createDto.Email))
            {
      return BadRequest(new { message = "Email já cadastrado" });
       }

     var user = new User
   {
         Nome = createDto.Nome,
          Email = createDto.Email,
            SenhaHash = HashPassword(createDto.Senha),
 Role = createDto.Role,
        CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Usuário criado com sucesso. ID: {Id}", user.Id);

    var userDto = MapToDto(user);
return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
     }

   /// <summary>
 /// Atualiza um usuário existente
        /// </summary>
  [HttpPut("{id}")]
  [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto updateDto)
 {
            _logger.LogInformation("Atualizando usuário com ID: {Id}", id);

  var user = await _context.Users.FindAsync(id);

       if (user == null)
{
     _logger.LogWarning("Usuário com ID {Id} não encontrado", id);
        return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
       }

  if (!string.IsNullOrEmpty(updateDto.Nome))
       user.Nome = updateDto.Nome;

  if (!string.IsNullOrEmpty(updateDto.Email))
    {
 // Verificar se novo email já existe
     if (await _context.Users.AnyAsync(u => u.Email == updateDto.Email && u.Id != id))
     {
         return BadRequest(new { message = "Email já cadastrado" });
   }
     user.Email = updateDto.Email;
    }

   if (!string.IsNullOrEmpty(updateDto.Senha))
          user.SenhaHash = HashPassword(updateDto.Senha);

         if (!string.IsNullOrEmpty(updateDto.Role))
    user.Role = updateDto.Role;

        await _context.SaveChangesAsync();

   _logger.LogInformation("Usuário com ID {Id} atualizado com sucesso", id);

            return Ok(MapToDto(user));
        }

        /// <summary>
        /// Exclui um usuário
        /// </summary>
[HttpDelete("{id}")]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation("Excluindo usuário com ID: {Id}", id);

   var user = await _context.Users.FindAsync(id);

 if (user == null)
     {
 _logger.LogWarning("Usuário com ID {Id} não encontrado", id);
          return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
 }

      _context.Users.Remove(user);
            await _context.SaveChangesAsync();

  _logger.LogInformation("Usuário com ID {Id} excluído com sucesso", id);

            return NoContent();
  }

        private UserDto MapToDto(User user)
        {
     return new UserDto
    {
                Id = user.Id,
     Nome = user.Nome,
     Email = user.Email,
      Role = user.Role,
    CreatedAt = user.CreatedAt,
      Links = new Dictionary<string, string>
   {
  { "self", $"/api/v1/users/{user.Id}" },
  { "update", $"/api/v1/users/{user.Id}" },
       { "delete", $"/api/v1/users/{user.Id}" },
     { "goals", $"/api/v1/users/{user.Id}/goals" },
   { "transactions", $"/api/v1/users/{user.Id}/transactions" }
                }
            };
        }

   private string HashPassword(string password)
        {
       // Simples hash SHA256 para demo
            // Em produção, use BCrypt ou Identity
  using var sha256 = SHA256.Create();
  var bytes = Encoding.UTF8.GetBytes(password);
   var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
      }
    }
}
