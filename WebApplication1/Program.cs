using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication1.Data;
using WebApplication1.HealthChecks;
using WebApplication1.Middleware;

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .CreateLogger();

try
{
    Log.Information("Iniciando aplicação");

    var builder = WebApplication.CreateBuilder(args);

    // Adicionar Serilog
    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddControllers();

    // Configurar Oracle Database
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Configurar Health Checks
    builder.Services.AddHealthChecks()
        .AddCheck<DatabaseHealthCheck>("database");

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Future of Work API",
            Version = "v1",
   Description = "API RESTful para gerenciamento de metas profissionais - O Futuro do Trabalho"
        });
c.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
    {
            Title = "Future of Work API",
    Version = "v2",
            Description = "API RESTful para gerenciamento de metas profissionais - O Futuro do Trabalho (Versão 2)"
        });
        
   // Configurar autenticação por API Key no Swagger
        c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
      {
        Description = "API Key needed to access the endpoints. X-API-Key: FiapGS2024SecureKey",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
     Name = "X-API-Key",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
 });

c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
      {
 {
          new Microsoft.OpenApi.Models.OpenApiSecurityScheme
   {
           Reference = new Microsoft.OpenApi.Models.OpenApiReference
   {
      Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
      Id = "ApiKey"
      }
       },
    Array.Empty<string>()
            }
   });
 });

    var app = builder.Build();

    // Configurar Swagger para todos os ambientes (Development e Production)
    app.UseSwagger();
    app.UseSwaggerUI(c =>
  {
     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Future of Work API V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Future of Work API V2");
   c.RoutePrefix = string.Empty; // Swagger na raiz (http://localhost:port/)
    });

    // Adicionar Serilog request logging
    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    // Adicionar API Key Middleware
    app.UseMiddleware<ApiKeyMiddleware>();

    app.UseAuthorization();

    // Configurar Health Check endpoint
    app.MapHealthChecks("/health");

    app.MapControllers();

    // Obter URLs onde a aplicação está rodando
    var urls = builder.Configuration["ASPNETCORE_URLS"] ?? "http://localhost:5000;https://localhost:5001";
    
    Log.Information("Aplicação iniciada com sucesso");
 Log.Information("==================================================");
    Log.Information("Swagger UI disponível em:");
    foreach (var url in urls.Split(';'))
    {
        Log.Information("  {Url}", url);
  }
    Log.Information("Health Check: {Url}/health", urls.Split(';')[0]);
    Log.Information("API Key: FiapGS2024SecureKey");
    Log.Information("==================================================");
    
    // Também imprimir no console diretamente
    Console.WriteLine("\n==================================================");
    Console.WriteLine("?? APLICAÇÃO INICIADA COM SUCESSO!");
    Console.WriteLine("==================================================");
    Console.WriteLine($"?? Swagger UI: {urls.Split(';')[0]}");
    Console.WriteLine($"?? Health Check: {urls.Split(';')[0]}/health");
    Console.WriteLine($"?? API Key: FiapGS2024SecureKey");
    Console.WriteLine("==================================================\n");
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicação falhou ao iniciar");
    Console.WriteLine($"\n? ERRO: {ex.Message}\n");
}
finally
{
    Log.CloseAndFlush();
}
