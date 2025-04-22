using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Persistence.Interceptors;

namespace TaskManagement.Persistence;

public static class PersistenceExtensions
{
    public static void AddPersistenceLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add Interceptors to DI
        services.AddScoped<PublishDomainInterceptor>();
        
        // Add Db;
        services.AddDbContext<AppDbContext>((sp, config) =>
        {
            var connectionString =
                configuration.GetConnectionString("DefaultConnection");

            config.UseSqlServer(connectionString);
            
            var domainInterceptor = sp.GetRequiredService<PublishDomainInterceptor>();
            config.AddInterceptors(domainInterceptor);
        });
    }
} 