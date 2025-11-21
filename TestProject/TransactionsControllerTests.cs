using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using WebApplication1.Controllers.V1;
using WebApplication1.Data;
using WebApplication1.Models;

namespace TestProject
{
    public class TransactionsControllerTests : IDisposable
    {
  private readonly AppDbContext _context;
     private readonly TransactionsController _controller;
        private readonly Mock<ILogger<TransactionsController>> _loggerMock;
        private User _testUser = null!;
        private Category _testCategory = null!;

        public TransactionsControllerTests()
 {
       var options = new DbContextOptionsBuilder<AppDbContext>()
   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
 .Options;

    _context = new AppDbContext(options);
   _loggerMock = new Mock<ILogger<TransactionsController>>();
       _controller = new TransactionsController(_context, _loggerMock.Object);

   // Setup test data
  SetupTestData().Wait();
        }

        private async Task SetupTestData()
        {
   _testUser = new User { Nome = "Test User", Email = "test@test.com", SenhaHash = "hash", Role = "USER" };
  _testCategory = new Category { Nome = "Test Category", Tipo = "DESPESA" };
   
    _context.Users.Add(_testUser);
    _context.Categories.Add(_testCategory);
     await _context.SaveChangesAsync();
     }

   [Fact]
   public async Task GetTransactions_ReturnsPagedResult()
  {
 // Arrange
    _context.Transactions.Add(new Transaction 
   { 
    IdUser = _testUser.Id,
     IdCategory = _testCategory.Id,
      Tipo = "DESPESA",
    Valor = 100,
      DataTransacao = DateTime.Now
 });
     _context.Transactions.Add(new Transaction 
    { 
  IdUser = _testUser.Id,
    IdCategory = _testCategory.Id,
    Tipo = "RECEITA",
 Valor = 500,
    DataTransacao = DateTime.Now
 });
await _context.SaveChangesAsync();

     // Act
         var result = await _controller.GetTransactions(1, 10);

// Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
       var pagedResult = Assert.IsType<PagedResult<TransactionDto>>(okResult.Value);
       Assert.Equal(2, pagedResult.TotalCount);
 }

  [Fact]
 public async Task GetTransactions_WithTipoFilter_ReturnsFilteredResults()
        {
   // Arrange
  _context.Transactions.Add(new Transaction 
   { 
 IdUser = _testUser.Id,
    IdCategory = _testCategory.Id,
     Tipo = "DESPESA",
 Valor = 100,
   DataTransacao = DateTime.Now
   });
         _context.Transactions.Add(new Transaction 
     { 
      IdUser = _testUser.Id,
   IdCategory = _testCategory.Id,
  Tipo = "RECEITA",
     Valor = 500,
     DataTransacao = DateTime.Now
     });
    await _context.SaveChangesAsync();

  // Act
     var result = await _controller.GetTransactions(1, 10, "DESPESA");

 // Assert
  var okResult = Assert.IsType<OkObjectResult>(result.Result);
  var pagedResult = Assert.IsType<PagedResult<TransactionDto>>(okResult.Value);
       Assert.Equal(1, pagedResult.TotalCount);
    Assert.Equal("DESPESA", pagedResult.Items[0].Tipo);
 }

        [Fact]
        public async Task GetTransaction_WithValidId_ReturnsTransaction()
      {
       // Arrange
  var transaction = new Transaction
   {
   IdUser = _testUser.Id,
  IdCategory = _testCategory.Id,
    Tipo = "DESPESA",
       Valor = 150.50m,
   Descricao = "Test Transaction",
       DataTransacao = DateTime.Now
   };
     _context.Transactions.Add(transaction);
  await _context.SaveChangesAsync();

   // Act
       var result = await _controller.GetTransaction(transaction.Id);

      // Assert
  var okResult = Assert.IsType<OkObjectResult>(result.Result);
var transactionDto = Assert.IsType<TransactionDto>(okResult.Value);
       Assert.Equal(150.50m, transactionDto.Valor);
     }

        [Fact]
      public async Task CreateTransaction_WithValidData_ReturnsCreatedTransaction()
 {
// Arrange
  var createDto = new CreateTransactionDto
      {
     IdUser = _testUser.Id,
IdCategory = _testCategory.Id,
  Tipo = "DESPESA",
     Valor = 200,
 Descricao = "Nova transação",
     Merchant = "Loja ABC",
       DataTransacao = DateTime.Now
  };

   // Act
          var result = await _controller.CreateTransaction(createDto);

   // Assert
     var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
   var transactionDto = Assert.IsType<TransactionDto>(createdResult.Value);
       Assert.Equal(200, transactionDto.Valor);
  Assert.Equal("DESPESA", transactionDto.Tipo);
        }

 [Fact]
  public async Task CreateTransaction_WithInvalidUser_ReturnsBadRequest()
  {
  // Arrange
   var createDto = new CreateTransactionDto
     {
    IdUser = 999, // Invalid user
  IdCategory = _testCategory.Id,
 Tipo = "DESPESA",
Valor = 100,
    DataTransacao = DateTime.Now
     };

// Act
var result = await _controller.CreateTransaction(createDto);

   // Assert
  var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
   Assert.NotNull(badRequestResult.Value);
 }

        [Fact]
    public async Task UpdateTransaction_WithValidData_ReturnsUpdatedTransaction()
     {
 // Arrange
   var transaction = new Transaction
   {
     IdUser = _testUser.Id,
   IdCategory = _testCategory.Id,
  Tipo = "DESPESA",
     Valor = 100,
 DataTransacao = DateTime.Now
      };
_context.Transactions.Add(transaction);
   await _context.SaveChangesAsync();

     var updateDto = new UpdateTransactionDto { Valor = 250, Descricao = "Atualizada" };

      // Act
            var result = await _controller.UpdateTransaction(transaction.Id, updateDto);

     // Assert
       var okResult = Assert.IsType<OkObjectResult>(result.Result);
   var transactionDto = Assert.IsType<TransactionDto>(okResult.Value);
   Assert.Equal(250, transactionDto.Valor);
       Assert.Equal("Atualizada", transactionDto.Descricao);
 }

        [Fact]
 public async Task DeleteTransaction_WithValidId_ReturnsNoContent()
   {
   // Arrange
 var transaction = new Transaction
 {
    IdUser = _testUser.Id,
  IdCategory = _testCategory.Id,
  Tipo = "DESPESA",
Valor = 100,
     DataTransacao = DateTime.Now
   };
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

       // Act
     var result = await _controller.DeleteTransaction(transaction.Id);

    // Assert
  Assert.IsType<NoContentResult>(result);
   Assert.Null(await _context.Transactions.FindAsync(transaction.Id));
 }

[Fact]
public async Task GetTransaction_WithInvalidId_ReturnsNotFound()
 {
// Act
   var result = await _controller.GetTransaction(999);

// Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
    }

        public void Dispose()
        {
   _context.Database.EnsureDeleted();
   _context.Dispose();
        }
    }
}
