using KarasKino.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace KarasKino.FunctionalTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
  private PostgreSqlContainer? _dbContainer;

  public async Task InitializeAsync()
  {
    _dbContainer = new PostgreSqlBuilder()
      .WithImage("postgres:latest")
      .WithPassword("Your_password123!")
      .Build();
    await _dbContainer.StartAsync();
  }

  public new async Task DisposeAsync()
  {
    if (_dbContainer != null)
    {
      await _dbContainer.DisposeAsync();
    }
  }

  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.UseEnvironment("Testing");
    var host = builder.Build();
    host.Start();

    var serviceProvider = host.Services;

    using (var scope = serviceProvider.CreateScope())
    {
      var scopedServices = scope.ServiceProvider;
      var db = scopedServices.GetRequiredService<AppDbContext>();
      var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

      try
      {
        db.Database.Migrate();
        SeedData.InitializeAsync(db).Wait();
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "An error occurred seeding the database. Error: {exceptionMessage}", ex.Message);
      }
    }

    return host;
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder
      .ConfigureAppConfiguration((context, config) =>
      {
        config.AddInMemoryCollection(new Dictionary<string, string?>
        {
          ["ConnectionStrings:DefaultConnection"] = _dbContainer!.GetConnectionString()
        });
      })
      .ConfigureServices(services =>
      {
        // Remove the app's DbContext registration
        var descriptors = services.Where(
          d => d.ServiceType == typeof(AppDbContext) ||
               d.ServiceType == typeof(DbContextOptions<AppDbContext>))
          .ToList();

        foreach (var descriptor in descriptors)
        {
          services.Remove(descriptor);
        }

        // Replace with Postgres Testcontainer instance
        services.AddDbContext<AppDbContext>((provider, options) =>
        {
          options.UseNpgsql(_dbContainer!.GetConnectionString());
          var interceptor = provider.GetRequiredService<EventDispatchInterceptor>();
          options.AddInterceptors(interceptor);
        });
      });
  }
}
