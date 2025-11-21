using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class GoalsController : ControllerBase
    {
     private readonly AppDbContext _context;
  private readonly ILogger<GoalsController> _logger;

        public GoalsController(AppDbContext context, ILogger<GoalsController> logger)
        {
     _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todas as metas com paginação
        /// </summary>
   [HttpGet]
        [ProducesResponseType(typeof(PagedResult<GoalDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<GoalDto>>> GetGoals(
            [FromQuery] int pageNumber = 1,
   [FromQuery] int pageSize = 10,
            [FromQuery] string? status = null,
  [FromQuery] int? idUser = null)
 {
         _logger.LogInformation("Buscando metas - Página: {PageNumber}, Tamanho: {PageSize}", pageNumber, pageSize);

    var query = _context.Goals
      .Include(g => g.User)
       .AsQueryable();

  if (!string.IsNullOrEmpty(status))
    query = query.Where(g => g.Status == status);

  if (idUser.HasValue)
           query = query.Where(g => g.IdUser == idUser.Value);

  var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

     var goals = await query
       .OrderByDescending(g => g.CreatedAt)
        .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
       .ToListAsync();

            var goalDtos = goals.Select(g => MapToDto(g)).ToList();

          var result = new PagedResult<GoalDto>
            {
        Items = goalDtos,
     PageNumber = pageNumber,
             PageSize = pageSize,
   TotalPages = totalPages,
                TotalCount = totalCount
            };

   return Ok(result);
        }

        /// <summary>
        /// Obtém uma meta específica por ID
     /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GoalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GoalDto>> GetGoal(int id)
        {
            _logger.LogInformation("Buscando meta com ID: {Id}", id);

        var goal = await _context.Goals
 .Include(g => g.User)
          .FirstOrDefaultAsync(g => g.Id == id);

        if (goal == null)
          {
    _logger.LogWarning("Meta com ID {Id} não encontrada", id);
                return NotFound(new { message = $"Meta com ID {id} não encontrada" });
      }

       return Ok(MapToDto(goal));
}

    /// <summary>
        /// Cria uma nova meta
/// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(GoalDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GoalDto>> CreateGoal([FromBody] CreateGoalDto createDto)
    {
            _logger.LogInformation("Criando nova meta: {Titulo}", createDto.Titulo);

            if (!ModelState.IsValid)
   return BadRequest(ModelState);

   // Validar usuário existe (se fornecido)
       if (createDto.IdUser.HasValue && !await _context.Users.AnyAsync(u => u.Id == createDto.IdUser))
                return BadRequest(new { message = "Usuário não encontrado" });

            var goal = new Goal
            {
        IdUser = createDto.IdUser,
     Titulo = createDto.Titulo,
Tipo = createDto.Tipo,
  ValorAlvo = createDto.ValorAlvo,
  DiasAlvo = createDto.DiasAlvo,
        DiasConcluidos = 0,
           QtdAlvoDiaria = createDto.QtdAlvoDiaria,
                Unidade = createDto.Unidade,
     DataInicio = createDto.DataInicio,
       DataFim = createDto.DataFim,
                Status = "ATIVA",
      CreatedAt = DateTime.Now
         };

 _context.Goals.Add(goal);
    await _context.SaveChangesAsync();

         _logger.LogInformation("Meta criada com sucesso. ID: {Id}", goal.Id);

          // Recarregar com includes
            goal = await _context.Goals
  .Include(g => g.User)
         .FirstAsync(g => g.Id == goal.Id);

        var goalDto = MapToDto(goal);
            return CreatedAtAction(nameof(GetGoal), new { id = goal.Id }, goalDto);
        }

      /// <summary>
        /// Atualiza uma meta existente
        /// </summary>
 [HttpPut("{id}")]
        [ProducesResponseType(typeof(GoalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GoalDto>> UpdateGoal(int id, [FromBody] UpdateGoalDto updateDto)
        {
_logger.LogInformation("Atualizando meta com ID: {Id}", id);

            var goal = await _context.Goals
    .Include(g => g.User)
         .FirstOrDefaultAsync(g => g.Id == id);

    if (goal == null)
       {
     _logger.LogWarning("Meta com ID {Id} não encontrada para atualização", id);
     return NotFound(new { message = $"Meta com ID {id} não encontrada" });
            }

if (!string.IsNullOrEmpty(updateDto.Titulo))
         goal.Titulo = updateDto.Titulo;

      if (!string.IsNullOrEmpty(updateDto.Tipo))
  goal.Tipo = updateDto.Tipo;

            if (updateDto.ValorAlvo.HasValue)
     goal.ValorAlvo = updateDto.ValorAlvo;

   if (updateDto.DiasAlvo.HasValue)
             goal.DiasAlvo = updateDto.DiasAlvo;

 if (updateDto.DiasConcluidos.HasValue)
    goal.DiasConcluidos = updateDto.DiasConcluidos.Value;

          if (updateDto.QtdAlvoDiaria.HasValue)
           goal.QtdAlvoDiaria = updateDto.QtdAlvoDiaria;

            if (updateDto.Unidade != null)
            goal.Unidade = updateDto.Unidade;

   if (updateDto.DataInicio.HasValue)
          goal.DataInicio = updateDto.DataInicio;

         if (updateDto.DataFim.HasValue)
      goal.DataFim = updateDto.DataFim;

            if (!string.IsNullOrEmpty(updateDto.Status))
  goal.Status = updateDto.Status;

       await _context.SaveChangesAsync();

     _logger.LogInformation("Meta com ID {Id} atualizada com sucesso", id);

        return Ok(MapToDto(goal));
        }

    /// <summary>
        /// Exclui uma meta
        /// </summary>
    [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
 public async Task<IActionResult> DeleteGoal(int id)
     {
            _logger.LogInformation("Excluindo meta com ID: {Id}", id);

            var goal = await _context.Goals.FindAsync(id);

            if (goal == null)
          {
      _logger.LogWarning("Meta com ID {Id} não encontrada para exclusão", id);
       return NotFound(new { message = $"Meta com ID {id} não encontrada" });
            }

            _context.Goals.Remove(goal);
          await _context.SaveChangesAsync();

        _logger.LogInformation("Meta com ID {Id} excluída com sucesso", id);

  return NoContent();
        }

        private GoalDto MapToDto(Goal goal)
 {
            var dto = new GoalDto
    {
    Id = goal.Id,
                IdUser = goal.IdUser,
 Titulo = goal.Titulo,
            Tipo = goal.Tipo,
                ValorAlvo = goal.ValorAlvo,
           DiasAlvo = goal.DiasAlvo,
    DiasConcluidos = goal.DiasConcluidos,
      QtdAlvoDiaria = goal.QtdAlvoDiaria,
           Unidade = goal.Unidade,
   DataInicio = goal.DataInicio,
     DataFim = goal.DataFim,
          Status = goal.Status,
          CreatedAt = goal.CreatedAt,
     UserNome = goal.User?.Nome,
     Links = new Dictionary<string, string>
    {
       { "self", $"/api/v1/goals/{goal.Id}" },
     { "update", $"/api/v1/goals/{goal.Id}" },
           { "delete", $"/api/v1/goals/{goal.Id}" },
     { "transactions", $"/api/v1/goals/{goal.Id}/transactions" }
 }
          };

            if (goal.IdUser.HasValue)
{
       dto.Links.Add("user", $"/api/v1/users/{goal.IdUser}");
 }

            return dto;
 }
    }
}
