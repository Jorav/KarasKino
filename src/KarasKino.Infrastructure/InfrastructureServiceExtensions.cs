using KarasKino.Application.Contributors.List;
using KarasKino.Core.Interfaces;
using KarasKino.Core.Services;
using KarasKino.Infrastructure.Data;
using KarasKino.Infrastructure.Data.Queries;
using KarasKino.Infrastructure.Movies;

namespace KarasKino.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    // Aspire injects "karaskinodb" automatically via .WithReference(cleanArchDb)
    // "DefaultConnection" is the fallback for running without Aspire
    string? connectionString = config.GetConnectionString("karaskinodb")
                               ?? config.GetConnectionString("DefaultConnection");
    Guard.Against.Null(connectionString);

    services.AddScoped<EventDispatchInterceptor>();
    services.AddScoped<IDomainEventDispatcher, MediatorDomainEventDispatcher>();

    services.AddDbContext<AppDbContext>((provider, options) =>
    {
      var interceptor = provider.GetRequiredService<EventDispatchInterceptor>();
      options.UseNpgsql(connectionString);
      options.AddInterceptors(interceptor);
    });

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
            .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
            .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
            .AddScoped<IDeleteContributorService, DeleteContributorService>();
    services.AddMovieServices(config);

    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
