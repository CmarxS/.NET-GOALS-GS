using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApplication1.Middleware;

namespace TestProject
{
    public class ApiKeyMiddlewareTests
    {
      private readonly Mock<RequestDelegate> _nextMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly ApiKeyMiddleware _middleware;

  public ApiKeyMiddlewareTests()
        {
            _nextMock = new Mock<RequestDelegate>();
       _configMock = new Mock<IConfiguration>();

            // Setup API Key configuration
            _configMock.Setup(c => c["ApiSettings:ApiKey"]).Returns("FiapGS2025SecureKey");
   
         _middleware = new ApiKeyMiddleware(_nextMock.Object);
        }

        [Fact]
        public async Task InvokeAsync_SwaggerPath_AllowsAccess()
   {
            // Arrange
 var context = new DefaultHttpContext();
          context.Request.Path = "/swagger";
 context.RequestServices = CreateServiceProvider();

            // Act
            await _middleware.InvokeAsync(context);

 // Assert
        _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_HealthPath_AllowsAccess()
        {
      // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "/health";
            context.RequestServices = CreateServiceProvider();

      // Act
    await _middleware.InvokeAsync(context);

            // Assert
   _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_ApiPathWithoutKey_Returns401()
        {
      // Arrange
            var context = new DefaultHttpContext();
 context.Request.Path = "/api/v1/users";
     context.Response.Body = new MemoryStream();
  context.RequestServices = CreateServiceProvider();

       // Act
   await _middleware.InvokeAsync(context);

         // Assert
            Assert.Equal(401, context.Response.StatusCode);
   _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Never);
        }

        [Fact]
        public async Task InvokeAsync_ApiPathWithValidKey_AllowsAccess()
      {
         // Arrange
  var context = new DefaultHttpContext();
            context.Request.Path = "/api/v1/users";
context.Request.Headers["X-API-Key"] = "FiapGS2025SecureKey";
   context.RequestServices = CreateServiceProvider();

   // Act
    await _middleware.InvokeAsync(context);

            // Assert
            _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_ApiPathWithInvalidKey_Returns401()
  {
            // Arrange
            var context = new DefaultHttpContext();
          context.Request.Path = "/api/v1/users";
     context.Request.Headers["X-API-Key"] = "InvalidKey";
      context.Response.Body = new MemoryStream();
 context.RequestServices = CreateServiceProvider();

        // Act
       await _middleware.InvokeAsync(context);

    // Assert
 Assert.Equal(401, context.Response.StatusCode);
            _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Never);
        }

      [Fact]
   public async Task InvokeAsync_NonApiPath_AllowsAccess()
        {
  // Arrange
            var context = new DefaultHttpContext();
          context.Request.Path = "/home";
    context.RequestServices = CreateServiceProvider();

// Act
            await _middleware.InvokeAsync(context);

     // Assert
   _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        }

        private IServiceProvider CreateServiceProvider()
        {
        var services = new ServiceCollection();
   services.AddSingleton(_configMock.Object);
      return services.BuildServiceProvider();
        }
    }
}
