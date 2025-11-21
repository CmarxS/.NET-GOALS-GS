using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApplication1.Data;

namespace WebApplication1.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
  private readonly AppDbContext _context;

 public DatabaseHealthCheck(AppDbContext context)
        {
     _context = context;
    }

   public async Task<HealthCheckResult> CheckHealthAsync(
 HealthCheckContext context,
    CancellationToken cancellationToken = default)
   {
 try
   {
        await _context.Database.CanConnectAsync(cancellationToken);
   return HealthCheckResult.Healthy("Banco de dados está acessível");
       }
  catch (Exception ex)
       {
    return HealthCheckResult.Unhealthy("Banco de dados não está acessível", ex);
   }
        }
    }
}
