using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using WebApplication1.Controllers.V1;
using WebApplication1.Data;
using WebApplication1.Models;

namespace TestProject
{
    public class CategoriesControllerTests : IDisposable
    {
   private readonly AppDbContext _context;
    private readonly CategoriesController _controller;
        private readonly Mock<ILogger<CategoriesController>> _loggerMock;

        public CategoriesControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;

      _context = new AppDbContext(options);
     _loggerMock = new Mock<ILogger<CategoriesController>>();
  _controller = new CategoriesController(_context, _loggerMock.Object);
 }

        [Fact]
 public async Task GetCategories_ReturnsPagedResult()
 {
      // Arrange
         _context.Categories.Add(new Category { Nome = "Alimentação", Tipo = "DESPESA", LimiteMensal = 800 });
            _context.Categories.Add(new Category { Nome = "Salário", Tipo = "RECEITA" });
   await _context.SaveChangesAsync();

   // Act
      var result = await _controller.GetCategories(1, 10);

            // Assert
var okResult = Assert.IsType<OkObjectResult>(result.Result);
var pagedResult = Assert.IsType<PagedResult<CategoryDto>>(okResult.Value);
       Assert.Equal(2, pagedResult.TotalCount);
   }

        [Fact]
        public async Task GetCategories_WithTipoFilter_ReturnsFilteredResults()
 {
            // Arrange
    _context.Categories.Add(new Category { Nome = "Alimentação", Tipo = "DESPESA" });
   _context.Categories.Add(new Category { Nome = "Salário", Tipo = "RECEITA" });
 await _context.SaveChangesAsync();

 // Act
   var result = await _controller.GetCategories(1, 10, "DESPESA");

      // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var pagedResult = Assert.IsType<PagedResult<CategoryDto>>(okResult.Value);
       Assert.Equal(1, pagedResult.TotalCount);
   Assert.Equal("DESPESA", pagedResult.Items[0].Tipo);
        }

        [Fact]
        public async Task GetCategory_WithValidId_ReturnsCategory()
        {
       // Arrange
      var category = new Category { Nome = "Test", Tipo = "DESPESA", LimiteMensal = 500 };
  _context.Categories.Add(category);
    await _context.SaveChangesAsync();

    // Act
   var result = await _controller.GetCategory(category.Id);

      // Assert
     var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var categoryDto = Assert.IsType<CategoryDto>(okResult.Value);
            Assert.Equal("Test", categoryDto.Nome);
   }

      [Fact]
        public async Task CreateCategory_WithValidData_ReturnsCreatedCategory()
        {
         // Arrange
     var createDto = new CreateCategoryDto
       {
  Nome = "Transporte",
    Tipo = "DESPESA",
     LimiteMensal = 400
 };

// Act
            var result = await _controller.CreateCategory(createDto);

    // Assert
    var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
       var categoryDto = Assert.IsType<CategoryDto>(createdResult.Value);
       Assert.Equal("Transporte", categoryDto.Nome);
         Assert.Equal("DESPESA", categoryDto.Tipo);
      }

     [Fact]
  public async Task CreateCategory_WithDuplicateName_ReturnsBadRequest()
 {
            // Arrange
 _context.Categories.Add(new Category { Nome = "Duplicada", Tipo = "DESPESA" });
      await _context.SaveChangesAsync();

      var createDto = new CreateCategoryDto
    {
    Nome = "Duplicada",
         Tipo = "DESPESA"
     };

     // Act
   var result = await _controller.CreateCategory(createDto);

  // Assert
     var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
   Assert.NotNull(badRequestResult.Value);
 }

        [Fact]
        public async Task UpdateCategory_WithValidData_ReturnsUpdatedCategory()
 {
  // Arrange
  var category = new Category { Nome = "Original", Tipo = "DESPESA" };
  _context.Categories.Add(category);
  await _context.SaveChangesAsync();

       var updateDto = new UpdateCategoryDto { LimiteMensal = 1000 };

      // Act
   var result = await _controller.UpdateCategory(category.Id, updateDto);

        // Assert
  var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var categoryDto = Assert.IsType<CategoryDto>(okResult.Value);
 Assert.Equal(1000, categoryDto.LimiteMensal);
        }

        [Fact]
public async Task DeleteCategory_WithValidId_ReturnsNoContent()
        {
      // Arrange
   var category = new Category { Nome = "Delete", Tipo = "DESPESA" };
  _context.Categories.Add(category);
       await _context.SaveChangesAsync();

      // Act
       var result = await _controller.DeleteCategory(category.Id);

   // Assert
       Assert.IsType<NoContentResult>(result);
       Assert.Null(await _context.Categories.FindAsync(category.Id));
        }

      public void Dispose()
        {
      _context.Database.EnsureDeleted();
   _context.Dispose();
        }
    }
}
