using Ardalis.Specification;
using KarasKino.Infrastructure.Data;
using KarasKino.Infrastructure.Movies;

namespace KarasKino.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    string? connectionString = config.GetConnectionString("karaskinodb")
                               ?? config.GetConnectionString("DefaultConnection");
    if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgresql://"))
    {
      connectionString = ConvertPostgresUrlToConnectionString(connectionString);
    }
    Guard.Against.Null(connectionString);

    services.AddScoped<EventDispatchInterceptor>();
    services.AddScoped<IDomainEventDispatcher, MediatorDomainEventDispatcher>();

    services.AddDbContext<AppDbContext>((provider, options) =>
    {
      var interceptor = provider.GetRequiredService<EventDispatchInterceptor>();
      options.UseNpgsql(connectionString);
      options.AddInterceptors(interceptor);
    });

    services.AddScoped(typeof(Core.Interfaces.IRepository<>), typeof(Repository<>))
            .AddScoped(typeof(IRepositoryBase<>), typeof(AppRepository<>));
    services.AddMovieServices(config);

    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }

  private static string ConvertPostgresUrlToConnectionString(string url)
  {
    var uri = new Uri(url);
    var userInfo = uri.UserInfo.Split(':');
    return $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
  }
}
