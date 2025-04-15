using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManagement.Infra;

public static class InfraExtensions
{
    public static void AddInfraLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(config =>
        {
            var connectionString =
                configuration.GetConnectionString("DefaultConnection");

            config.UseSqlServer(connectionString);
        });
    }
}