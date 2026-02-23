using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NEXUS.Data;
using NEXUS.Data.Configuration;

namespace NEXUS.Services;

public class DatabaseSeederHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseSeederHostedService> _logger;

    public DatabaseSeederHostedService(
        IServiceProvider serviceProvider,
        ILogger<DatabaseSeederHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await DatabaseSeeder.SeedDataAsync(_serviceProvider, _logger);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
