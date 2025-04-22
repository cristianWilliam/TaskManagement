using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Persistence;
using Testcontainers.MsSql;

namespace TaskManagementApi.Tests.Integration;

public class AppFactory : WebApplicationFactory<ApiProgram>, IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithCleanUp(true)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.Sources.Clear();

            var inMemoryCollection = new Dictionary<string, string>
                { { "ConnectionStrings:DefaultConnection", _container.GetConnectionString() } };
            config.AddInMemoryCollection(inMemoryCollection!);
        });

        base.ConfigureWebHost(builder);
    }

    private async Task RecreateDbAsync()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        await RecreateDbAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();
        await base.DisposeAsync();
    }
}

[CollectionDefinition("AppDb")]
public class AppFixture : ICollectionFixture<AppFactory>
{
}