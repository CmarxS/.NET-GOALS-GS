using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using WebApplication1.Controllers.V1;
using WebApplication1.Data;
using WebApplication1.Models;

namespace TestProject
{
    public class UsersControllerTests : IDisposable
    {
        private readonly AppDbContext _context;
 private readonly UsersController _controller;
        private readonly Mock<ILogger<UsersController>> _loggerMock;

        public UsersControllerTests()
        {
            // Configurar banco de dados em memória
var options = new DbContextOptionsBuilder<AppDbContext>()
  .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

       _context = new AppDbContext(options);
            _loggerMock = new Mock<ILogger<UsersController>>();
   _controller = new UsersController(_context, _loggerMock.Object);
        }

        [Fact]
        public async Task GetUsers_ReturnsPagedResult()
        {
            // Arrange
            _context.Users.Add(new User { Nome = "João", Email = "joao@test.com", SenhaHash = "hash1", Role = "USER" });
       _context.Users.Add(new User { Nome = "Maria", Email = "maria@test.com", SenhaHash = "hash2", Role = "USER" });
   await _context.SaveChangesAsync();

      // Act
            var result = await _controller.GetUsers(1, 10);

      // Assert
       var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagedResult = Assert.IsType<PagedResult<UserDto>>(okResult.Value);
         Assert.Equal(2, pagedResult.TotalCount);
            Assert.Equal(2, pagedResult.Items.Count);
  }

        [Fact]
    public async Task GetUser_WithValidId_ReturnsUser()
 {
       // Arrange
       var user = new User { Nome = "Test", Email = "test@test.com", SenhaHash = "hash", Role = "USER" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

       // Act
    var result = await _controller.GetUser(user.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
       var userDto = Assert.IsType<UserDto>(okResult.Value);
 Assert.Equal("Test", userDto.Nome);
            Assert.Equal("test@test.com", userDto.Email);
   }

      [Fact]
        public async Task GetUser_WithInvalidId_ReturnsNotFound()
        {
     // Act
            var result = await _controller.GetUser(999);

        // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
       Assert.NotNull(notFoundResult.Value);
        }

        [Fact]
        public async Task CreateUser_WithValidData_ReturnsCreatedUser()
 {
            // Arrange
     var createDto = new CreateUserDto
  {
  Nome = "Novo Usuário",
         Email = "novo@test.com",
       Senha = "senha123",
                Role = "USER"
    };

      // Act
  var result = await _controller.CreateUser(createDto);

 // Assert
   var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
    var userDto = Assert.IsType<UserDto>(createdResult.Value);
   Assert.Equal("Novo Usuário", userDto.Nome);
            Assert.Equal("novo@test.com", userDto.Email);
}

        [Fact]
   public async Task CreateUser_WithDuplicateEmail_ReturnsBadRequest()
  {
    // Arrange
            _context.Users.Add(new User { Nome = "Existente", Email = "existe@test.com", SenhaHash = "hash", Role = "USER" });
         await _context.SaveChangesAsync();

var createDto = new CreateUserDto
    {
 Nome = "Novo",
      Email = "existe@test.com",
    Senha = "senha123",
  Role = "USER"
    };

      // Act
         var result = await _controller.CreateUser(createDto);

            // Assert
  var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateUser_WithValidData_ReturnsUpdatedUser()
        {
       // Arrange
            var user = new User { Nome = "Original", Email = "original@test.com", SenhaHash = "hash", Role = "USER" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

var updateDto = new UpdateUserDto { Nome = "Atualizado" };

            // Act
            var result = await _controller.UpdateUser(user.Id, updateDto);

         // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
 var userDto = Assert.IsType<UserDto>(okResult.Value);
  Assert.Equal("Atualizado", userDto.Nome);
     }

      [Fact]
        public async Task DeleteUser_WithValidId_ReturnsNoContent()
        {
         // Arrange
      var user = new User { Nome = "Delete", Email = "delete@test.com", SenhaHash = "hash", Role = "USER" };
       _context.Users.Add(user);
     await _context.SaveChangesAsync();

       // Act
var result = await _controller.DeleteUser(user.Id);

        // Assert
Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.Users.FindAsync(user.Id));
        }

    public void Dispose()
        {
   _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
