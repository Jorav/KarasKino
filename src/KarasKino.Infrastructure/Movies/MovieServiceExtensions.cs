using KarasKino.Core.Movies.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace KarasKino.Infrastructure.Movies;

public static class MovieServiceExtensions
{
  public static IServiceCollection AddMovieServices(
    this IServiceCollection services,
    ConfigurationManager config)
  {
    services.AddOptions<TmdbConfiguration>()
            .BindConfiguration(TmdbConfiguration.SectionName)
            .ValidateDataAnnotations();

    services.AddHttpClient<TmdbClient>();
    services.AddScoped<ITmdbService, TmdbService>();

    return services;
  }
}
