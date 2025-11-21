using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using WebApplication1.Controllers.V1;
using WebApplication1.Data;
using WebApplication1.Models;

namespace TestProject
{
 public class GoalsControllerTests : IDisposable
    {
 private readonly AppDbContext _context;
  private readonly GoalsController _controller;
   private readonly Mock<ILogger<GoalsController>> _loggerMock;

        public GoalsControllerTests()
        {
   var options = new DbContextOptionsBuilder<AppDbContext>()
 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
 .Options;

   _context = new AppDbContext(options);
      _loggerMock = new Mock<ILogger<GoalsController>>();
   _controller = new GoalsController(_context, _loggerMock.Object);
  }

  [Fact]
        public async Task GetGoals_ReturnsPagedResult()
      {
     // Arrange
   _context.Goals.Add(new Goal { Titulo = "Meta 1", Tipo = "FINANCEIRO", Status = "ATIVA" });
       _context.Goals.Add(new Goal { Titulo = "Meta 2", Tipo = "HABITO", Status = "ATIVA" });
       await _context.SaveChangesAsync();

 // Act
       var result = await _controller.GetGoals(1, 10);

// Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var pagedResult = Assert.IsType<PagedResult<GoalDto>>(okResult.Value);
    Assert.Equal(2, pagedResult.TotalCount);
        }

[Fact]
        public async Task GetGoals_WithStatusFilter_ReturnsFilteredResults()
 {
       // Arrange
       _context.Goals.Add(new Goal { Titulo = "Ativa", Tipo = "FINANCEIRO", Status = "ATIVA" });
     _context.Goals.Add(new Goal { Titulo = "Concluída", Tipo = "FINANCEIRO", Status = "CONCLUIDA" });
   await _context.SaveChangesAsync();

   // Act
       var result = await _controller.GetGoals(1, 10, "ATIVA");

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
     var pagedResult = Assert.IsType<PagedResult<GoalDto>>(okResult.Value);
       Assert.Equal(1, pagedResult.TotalCount);
   Assert.Equal("ATIVA", pagedResult.Items[0].Status);
 }

        [Fact]
    public async Task GetGoal_WithValidId_ReturnsGoal()
        {
      // Arrange
   var goal = new Goal { Titulo = "Test Goal", Tipo = "FINANCEIRO", Status = "ATIVA", ValorAlvo = 10000 };
       _context.Goals.Add(goal);
    await _context.SaveChangesAsync();

      // Act
      var result = await _controller.GetGoal(goal.Id);

      // Assert
   var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var goalDto = Assert.IsType<GoalDto>(okResult.Value);
  Assert.Equal("Test Goal", goalDto.Titulo);
 }

        [Fact]
        public async Task CreateGoal_WithValidData_ReturnsCreatedGoal()
  {
            // Arrange
    var createDto = new CreateGoalDto
       {
   Titulo = "Nova Meta",
              Tipo = "FINANCEIRO",
     ValorAlvo = 5000,
    DataInicio = DateTime.Now,
DataFim = DateTime.Now.AddMonths(6)
       };

  // Act
var result = await _controller.CreateGoal(createDto);

// Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
       var goalDto = Assert.IsType<GoalDto>(createdResult.Value);
     Assert.Equal("Nova Meta", goalDto.Titulo);
Assert.Equal("ATIVA", goalDto.Status);
      }

   [Fact]
 public async Task CreateGoal_ForHabit_SetsCorrectType()
 {
   // Arrange
    var createDto = new CreateGoalDto
    {
       Titulo = "Exercício Diário",
    Tipo = "HABITO",
      DiasAlvo = 30,
     QtdAlvoDiaria = 1,
        Unidade = "sessão"
     };

            // Act
   var result = await _controller.CreateGoal(createDto);

    // Assert
   var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
       var goalDto = Assert.IsType<GoalDto>(createdResult.Value);
      Assert.Equal("HABITO", goalDto.Tipo);
   Assert.Equal(30, goalDto.DiasAlvo);
 }

        [Fact]
 public async Task UpdateGoal_WithValidData_ReturnsUpdatedGoal()
   {
            // Arrange
            var goal = new Goal { Titulo = "Original", Tipo = "FINANCEIRO", Status = "ATIVA" };
    _context.Goals.Add(goal);
        await _context.SaveChangesAsync();

var updateDto = new UpdateGoalDto 
{ 
    Status = "CONCLUIDA",
 DiasConcluidos = 30
};

// Act
 var result = await _controller.UpdateGoal(goal.Id, updateDto);

   // Assert
       var okResult = Assert.IsType<OkObjectResult>(result.Result);
 var goalDto = Assert.IsType<GoalDto>(okResult.Value);
  Assert.Equal("CONCLUIDA", goalDto.Status);
       Assert.Equal(30, goalDto.DiasConcluidos);
        }

 [Fact]
  public async Task DeleteGoal_WithValidId_ReturnsNoContent()
        {
 // Arrange
     var goal = new Goal { Titulo = "Delete", Tipo = "FINANCEIRO", Status = "ATIVA" };
      _context.Goals.Add(goal);
       await _context.SaveChangesAsync();

   // Act
  var result = await _controller.DeleteGoal(goal.Id);

// Assert
   Assert.IsType<NoContentResult>(result);
Assert.Null(await _context.Goals.FindAsync(goal.Id));
        }

        [Fact]
        public async Task GetGoal_WithInvalidId_ReturnsNotFound()
 {
    // Act
    var result = await _controller.GetGoal(999);

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
