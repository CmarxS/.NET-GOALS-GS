using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
 [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
      private readonly AppDbContext _context;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(AppDbContext context, ILogger<CategoriesController> logger)
   {
  _context = context;
        _logger = logger;
   }

        /// <summary>
        /// Obtém todas as categorias com paginação
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<CategoryDto>>> GetCategories(
            [FromQuery] int pageNumber = 1,
   [FromQuery] int pageSize = 10,
         [FromQuery] string? tipo = null)
        {
     _logger.LogInformation("Buscando categorias - Página: {PageNumber}", pageNumber);

      var query = _context.Categories.AsQueryable();

     if (!string.IsNullOrEmpty(tipo))
{
    query = query.Where(c => c.Tipo == tipo);
     }

         var totalCount = await query.CountAsync();
   var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

     var categories = await query
         .OrderBy(c => c.Nome)
   .Skip((pageNumber - 1) * pageSize)
     .Take(pageSize)
                .ToListAsync();

    var categoryDtos = categories.Select(c => MapToDto(c)).ToList();

       return Ok(new PagedResult<CategoryDto>
 {
        Items = categoryDtos,
        PageNumber = pageNumber,
    PageSize = pageSize,
   TotalPages = totalPages,
   TotalCount = totalCount
  });
 }

      /// <summary>
        /// Obtém uma categoria específica por ID
/// </summary>
      [HttpGet("{id}")]
   [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
 public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
    _logger.LogInformation("Buscando categoria com ID: {Id}", id);

  var category = await _context.Categories.FindAsync(id);

    if (category == null)
    {
       _logger.LogWarning("Categoria com ID {Id} não encontrada", id);
    return NotFound(new { message = $"Categoria com ID {id} não encontrada" });
        }

         return Ok(MapToDto(category));
 }

        /// <summary>
/// Cria uma nova categoria
 /// </summary>
   [HttpPost]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
 [ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryDto createDto)
 {
_logger.LogInformation("Criando nova categoria: {Nome}", createDto.Nome);

    if (!ModelState.IsValid)
     {
                return BadRequest(ModelState);
          }

   // Verificar se nome já existe
  if (await _context.Categories.AnyAsync(c => c.Nome == createDto.Nome))
     {
    return BadRequest(new { message = "Categoria com este nome já existe" });
     }

            var category = new Category
  {
 Nome = createDto.Nome,
  Tipo = createDto.Tipo,
    LimiteMensal = createDto.LimiteMensal,
     CreatedAt = DateTime.Now
   };

    _context.Categories.Add(category);
   await _context.SaveChangesAsync();

   _logger.LogInformation("Categoria criada com sucesso. ID: {Id}", category.Id);

  var categoryDto = MapToDto(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, categoryDto);
 }

        /// <summary>
  /// Atualiza uma categoria existente
        /// </summary>
        [HttpPut("{id}")]
  [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
 public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateDto)
        {
   _logger.LogInformation("Atualizando categoria com ID: {Id}", id);

   var category = await _context.Categories.FindAsync(id);

if (category == null)
       {
       _logger.LogWarning("Categoria com ID {Id} não encontrada", id);
     return NotFound(new { message = $"Categoria com ID {id} não encontrada" });
  }

   if (!string.IsNullOrEmpty(updateDto.Nome))
   {
   if (await _context.Categories.AnyAsync(c => c.Nome == updateDto.Nome && c.Id != id))
   {
      return BadRequest(new { message = "Categoria com este nome já existe" });
        }
       category.Nome = updateDto.Nome;
     }

   if (!string.IsNullOrEmpty(updateDto.Tipo))
    category.Tipo = updateDto.Tipo;

   if (updateDto.LimiteMensal.HasValue)
      category.LimiteMensal = updateDto.LimiteMensal;

          await _context.SaveChangesAsync();

 _logger.LogInformation("Categoria com ID {Id} atualizada com sucesso", id);

    return Ok(MapToDto(category));
 }

    /// <summary>
        /// Exclui uma categoria
  /// </summary>
        [HttpDelete("{id}")]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
 public async Task<IActionResult> DeleteCategory(int id)
   {
            _logger.LogInformation("Excluindo categoria com ID: {Id}", id);

 var category = await _context.Categories.FindAsync(id);

   if (category == null)
      {
 _logger.LogWarning("Categoria com ID {Id} não encontrada", id);
      return NotFound(new { message = $"Categoria com ID {id} não encontrada" });
            }

_context.Categories.Remove(category);
       await _context.SaveChangesAsync();

  _logger.LogInformation("Categoria com ID {Id} excluída com sucesso", id);

   return NoContent();
        }

  private CategoryDto MapToDto(Category category)
 {
   return new CategoryDto
     {
 Id = category.Id,
    Nome = category.Nome,
   Tipo = category.Tipo,
     LimiteMensal = category.LimiteMensal,
 CreatedAt = category.CreatedAt,
   Links = new Dictionary<string, string>
       {
  { "self", $"/api/v1/categories/{category.Id}" },
   { "update", $"/api/v1/categories/{category.Id}" },
    { "delete", $"/api/v1/categories/{category.Id}" },
    { "transactions", $"/api/v1/categories/{category.Id}/transactions" }
}
     };
 }
    }
}
