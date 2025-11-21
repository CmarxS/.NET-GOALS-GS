using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers.V1
{
[ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class TransactionsController : ControllerBase
    {
 private readonly AppDbContext _context;
 private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(AppDbContext context, ILogger<TransactionsController> logger)
   {
      _context = context;
      _logger = logger;
        }

        /// <summary>
     /// Obtém todas as transações com paginação
   /// </summary>
[HttpGet]
        [ProducesResponseType(typeof(PagedResult<TransactionDto>), StatusCodes.Status200OK)]
   public async Task<ActionResult<PagedResult<TransactionDto>>> GetTransactions(
  [FromQuery] int pageNumber = 1,
         [FromQuery] int pageSize = 10,
 [FromQuery] string? tipo = null,
      [FromQuery] int? idUser = null)
        {
     _logger.LogInformation("Buscando transações - Página: {PageNumber}", pageNumber);

         var query = _context.Transactions
       .Include(t => t.User)
   .Include(t => t.Category)
     .Include(t => t.Goal)
  .AsQueryable();

            if (!string.IsNullOrEmpty(tipo))
    query = query.Where(t => t.Tipo == tipo);

  if (idUser.HasValue)
    query = query.Where(t => t.IdUser == idUser.Value);

      var totalCount = await query.CountAsync();
      var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

      var transactions = await query
  .OrderByDescending(t => t.DataTransacao)
     .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
       .ToListAsync();

       var transactionDtos = transactions.Select(t => MapToDto(t)).ToList();

   return Ok(new PagedResult<TransactionDto>
{
          Items = transactionDtos,
 PageNumber = pageNumber,
 PageSize = pageSize,
      TotalPages = totalPages,
        TotalCount = totalCount
       });
        }

 /// <summary>
/// Obtém uma transação específica por ID
   /// </summary>
 [HttpGet("{id}")]
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
        {
 _logger.LogInformation("Buscando transação com ID: {Id}", id);

        var transaction = await _context.Transactions
   .Include(t => t.User)
   .Include(t => t.Category)
    .Include(t => t.Goal)
   .FirstOrDefaultAsync(t => t.Id == id);

   if (transaction == null)
   {
      _logger.LogWarning("Transação com ID {Id} não encontrada", id);
return NotFound(new { message = $"Transação com ID {id} não encontrada" });
   }

            return Ok(MapToDto(transaction));
        }

 /// <summary>
     /// Cria uma nova transação
/// </summary>
        [HttpPost]
 [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
   public async Task<ActionResult<TransactionDto>> CreateTransaction([FromBody] CreateTransactionDto createDto)
 {
  _logger.LogInformation("Criando nova transação para usuário: {IdUser}", createDto.IdUser);

     if (!ModelState.IsValid)
       return BadRequest(ModelState);

      // Validar usuário existe
   if (!await _context.Users.AnyAsync(u => u.Id == createDto.IdUser))
       return BadRequest(new { message = "Usuário não encontrado" });

     // Validar categoria existe
      if (!await _context.Categories.AnyAsync(c => c.Id == createDto.IdCategory))
  return BadRequest(new { message = "Categoria não encontrada" });

      // Validar meta existe (se fornecida)
 if (createDto.IdGoal.HasValue && !await _context.Goals.AnyAsync(g => g.Id == createDto.IdGoal))
     return BadRequest(new { message = "Meta não encontrada" });

            var transaction = new Transaction
  {
     IdUser = createDto.IdUser,
       IdCategory = createDto.IdCategory,
     IdGoal = createDto.IdGoal,
       Tipo = createDto.Tipo,
    Valor = createDto.Valor,
    Descricao = createDto.Descricao,
  Merchant = createDto.Merchant,
    DataTransacao = createDto.DataTransacao,
  CreatedAt = DateTime.Now
 };

    _context.Transactions.Add(transaction);
   await _context.SaveChangesAsync();

   _logger.LogInformation("Transação criada com sucesso. ID: {Id}", transaction.Id);

// Recarregar com includes para DTO
      transaction = await _context.Transactions
   .Include(t => t.User)
      .Include(t => t.Category)
   .Include(t => t.Goal)
       .FirstAsync(t => t.Id == transaction.Id);

     var transactionDto = MapToDto(transaction);
  return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transactionDto);
   }

        /// <summary>
   /// Atualiza uma transação existente
/// </summary>
     [HttpPut("{id}")]
   [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
     [ProducesResponseType(StatusCodes.Status404NotFound)]
   public async Task<ActionResult<TransactionDto>> UpdateTransaction(int id, [FromBody] UpdateTransactionDto updateDto)
   {
   _logger.LogInformation("Atualizando transação com ID: {Id}", id);

     var transaction = await _context.Transactions
    .Include(t => t.User)
    .Include(t => t.Category)
   .Include(t => t.Goal)
    .FirstOrDefaultAsync(t => t.Id == id);

   if (transaction == null)
   {
    _logger.LogWarning("Transação com ID {Id} não encontrada", id);
    return NotFound(new { message = $"Transação com ID {id} não encontrada" });
            }

     if (updateDto.IdCategory.HasValue)
 {
     if (!await _context.Categories.AnyAsync(c => c.Id == updateDto.IdCategory))
   return BadRequest(new { message = "Categoria não encontrada" });
     transaction.IdCategory = updateDto.IdCategory.Value;
}

       if (updateDto.IdGoal.HasValue && !await _context.Goals.AnyAsync(g => g.Id == updateDto.IdGoal))
    return BadRequest(new { message = "Meta não encontrada" });

  if (updateDto.IdGoal.HasValue)
    transaction.IdGoal = updateDto.IdGoal;

     if (!string.IsNullOrEmpty(updateDto.Tipo))
    transaction.Tipo = updateDto.Tipo;

  if (updateDto.Valor.HasValue)
    transaction.Valor = updateDto.Valor.Value;

       if (updateDto.Descricao != null)
       transaction.Descricao = updateDto.Descricao;

 if (updateDto.Merchant != null)
    transaction.Merchant = updateDto.Merchant;

      if (updateDto.DataTransacao.HasValue)
      transaction.DataTransacao = updateDto.DataTransacao.Value;

       await _context.SaveChangesAsync();

   _logger.LogInformation("Transação com ID {Id} atualizada com sucesso", id);

    return Ok(MapToDto(transaction));
        }

  /// <summary>
        /// Exclui uma transação
   /// </summary>
   [HttpDelete("{id}")]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
     public async Task<IActionResult> DeleteTransaction(int id)
        {
          _logger.LogInformation("Excluindo transação com ID: {Id}", id);

   var transaction = await _context.Transactions.FindAsync(id);

   if (transaction == null)
{
    _logger.LogWarning("Transação com ID {Id} não encontrada", id);
   return NotFound(new { message = $"Transação com ID {id} não encontrada" });
}

_context.Transactions.Remove(transaction);
  await _context.SaveChangesAsync();

  _logger.LogInformation("Transação com ID {Id} excluída com sucesso", id);

            return NoContent();
        }

      private TransactionDto MapToDto(Transaction transaction)
        {
     return new TransactionDto
     {
    Id = transaction.Id,
   IdUser = transaction.IdUser,
        IdCategory = transaction.IdCategory,
 IdGoal = transaction.IdGoal,
   Tipo = transaction.Tipo,
     Valor = transaction.Valor,
  Descricao = transaction.Descricao,
Merchant = transaction.Merchant,
     DataTransacao = transaction.DataTransacao,
    CreatedAt = transaction.CreatedAt,
          UserNome = transaction.User?.Nome,
    CategoryNome = transaction.Category?.Nome,
     GoalTitulo = transaction.Goal?.Titulo,
   Links = new Dictionary<string, string>
    {
      { "self", $"/api/v1/transactions/{transaction.Id}" },
       { "update", $"/api/v1/transactions/{transaction.Id}" },
    { "delete", $"/api/v1/transactions/{transaction.Id}" },
{ "user", $"/api/v1/users/{transaction.IdUser}" },
    { "category", $"/api/v1/categories/{transaction.IdCategory}" }
       }
       };
  }
    }
}
