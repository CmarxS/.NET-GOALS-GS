using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApplication1.Data;
using WebApplication1.HealthChecks;
using WebApplication1.Middleware;
using WebApplication1.Models;

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
    {
        options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

// Configurar Health Checks
    builder.Services.AddHealthChecks()
        .AddCheck<DatabaseHealthCheck>("database");

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
Title = "Future of Work API",
      Version = "v1",
      Description = "API RESTful para gerenciamento de metas profissionais - O Futuro do Trabalho"
    });
      
     c.SwaggerDoc("v2", new OpenApiInfo
    {
      Title = "Future of Work API",
        Version = "v2",
        Description = "API RESTful para gerenciamento de metas profissionais - O Futuro do Trabalho (Versão 2)"
        });
        
      // Configurar autenticação por API Key no Swagger
   c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
        {
            Description = "API Key needed to access the endpoints. X-API-Key: FiapGS2025SecureKey",
        In = ParameterLocation.Header,
 Name = "X-API-Key",
   Type = SecuritySchemeType.ApiKey
        });

     c.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
        {
        new OpenApiSecurityScheme
  {
    Reference = new OpenApiReference
   {
     Type = ReferenceType.SecurityScheme,
   Id = "ApiKey"
   }
     },
          Array.Empty<string>()
         }
});

        // Adicionar exemplos de requisição
        c.SchemaFilter<ExampleSchemaFilter>();
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
    Log.Information("API Key: FiapGS2025SecureKey");
    Log.Information("==================================================");
    
    // Também imprimir no console diretamente
    Console.WriteLine("\n==================================================");
    Console.WriteLine("🚀 APLICAÇÃO INICIADA COM SUCESSO!");
    Console.WriteLine("==================================================");
    Console.WriteLine($"📝 Swagger UI: {urls.Split(';')[0]}");
    Console.WriteLine($"🏥 Health Check: {urls.Split(';')[0]}/health");
    Console.WriteLine($"🔑 API Key: FiapGS2025SecureKey");
    Console.WriteLine("==================================================\n");
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicação falhou ao iniciar");
    Console.WriteLine($"\n❌ ERRO: {ex.Message}\n");
}
finally
{
    Log.CloseAndFlush();
}

// Filter para adicionar exemplos aos schemas do Swagger
public class ExampleSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
    if (context.Type == typeof(CreateUserDto))
      {
   schema.Example = new OpenApiObject
   {
["nome"] = new OpenApiString("João Silva"),
      ["email"] = new OpenApiString("joao.silva@email.com"),
    ["senha"] = new OpenApiString("Senha@123"),
     ["role"] = new OpenApiString("USER")
 };
  }
     else if (context.Type == typeof(CreateCategoryDto))
    {
       schema.Example = new OpenApiObject
          {
                ["nome"] = new OpenApiString("Alimentação"),
        ["tipo"] = new OpenApiString("DESPESA"),
        ["limiteMensal"] = new OpenApiDouble(800.00)
       };
        }
      else if (context.Type == typeof(CreateGoalDto))
{
        schema.Example = new OpenApiObject
      {
      ["idUser"] = new OpenApiInteger(1),
  ["titulo"] = new OpenApiString("Fundo de Emergência"),
         ["tipo"] = new OpenApiString("FINANCEIRO"),
    ["valorAlvo"] = new OpenApiDouble(10000.00),
       ["dataInicio"] = new OpenApiString("2024-11-20"),
["dataFim"] = new OpenApiString("2025-11-20")
            };
    }
else if (context.Type == typeof(CreateTransactionDto))
{
     schema.Example = new OpenApiObject
            {
       ["idUser"] = new OpenApiInteger(1),
     ["idCategory"] = new OpenApiInteger(1),
    ["idGoal"] = new OpenApiInteger(1),
     ["tipo"] = new OpenApiString("RECEITA"),
     ["valor"] = new OpenApiDouble(500.00),
   ["descricao"] = new OpenApiString("Aporte para fundo de emergência"),
    ["merchant"] = new OpenApiString("Banco XYZ"),
      ["dataTransacao"] = new OpenApiString("2024-11-20")
    };
     }
else if (context.Type == typeof(UpdateUserDto))
        {
   schema.Example = new OpenApiObject
   {
    ["nome"] = new OpenApiString("João Silva Atualizado"),
  ["email"] = new OpenApiString("joao.novo@email.com")
  };
    }
        else if (context.Type == typeof(UpdateCategoryDto))
      {
     schema.Example = new OpenApiObject
      {
      ["limiteMensal"] = new OpenApiDouble(1000.00)
       };
        }
else if (context.Type == typeof(UpdateGoalDto))
   {
          schema.Example = new OpenApiObject
      {
          ["status"] = new OpenApiString("CONCLUIDA"),
["diasConcluidos"] = new OpenApiInteger(30)
            };
        }
        else if (context.Type == typeof(UpdateTransactionDto))
      {
    schema.Example = new OpenApiObject
       {
      ["valor"] = new OpenApiDouble(600.00),
  ["descricao"] = new OpenApiString("Aporte atualizado")
  };
      }
    }
}
