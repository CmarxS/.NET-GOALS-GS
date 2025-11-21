using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.V2
{
    [ApiController]
    [Route("api/v2/[controller]")]
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
        /// V2: Obtém metas com informações resumidas e ordenação melhorada
     /// </summary>
  [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GoalDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<GoalDto>>> GetGoals(
   [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
     [FromQuery] string? orderBy = "date")
        {
         _logger.LogInformation("V2 - Buscando metas - Página: {PageNumber}", pageNumber);

       var query = _context.Goals
     .Include(g => g.User)
                .AsQueryable();

      // V2 Feature: Ordenação customizada
            query = orderBy?.ToLower() switch
  {
      "titulo" => query.OrderBy(g => g.Titulo),
                "status" => query.OrderBy(g => g.Status),
                "tipo" => query.OrderBy(g => g.Tipo),
          _ => query.OrderByDescending(g => g.CreatedAt)
       };

       var totalCount = await query.CountAsync();
    var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var goals = await query
    .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

            var goalDtos = goals.Select(g => new GoalDto
            {
      Id = g.Id,
       IdUser = g.IdUser,
       Titulo = g.Titulo,
        Tipo = g.Tipo,
     ValorAlvo = g.ValorAlvo,
        DiasAlvo = g.DiasAlvo,
                DiasConcluidos = g.DiasConcluidos,
    QtdAlvoDiaria = g.QtdAlvoDiaria,
                Unidade = g.Unidade,
                DataInicio = g.DataInicio,
                DataFim = g.DataFim,
   Status = g.Status,
                CreatedAt = g.CreatedAt,
         UserNome = g.User?.Nome,
          Links = new Dictionary<string, string>
              {
     { "self", $"/api/v2/goals/{g.Id}" }
         }
     }).ToList();

            return Ok(new PagedResult<GoalDto>
            {
                Items = goalDtos,
      PageNumber = pageNumber,
       PageSize = pageSize,
        TotalPages = totalPages,
        TotalCount = totalCount
   });
}
    }
}
