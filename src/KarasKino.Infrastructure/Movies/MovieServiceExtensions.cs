using KarasKino.Application.Movies.FindByImdbId;

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

    services.AddHttpClient("Tmdb");
    services.AddScoped<TmdbClient>();
    services.AddScoped<ITmdbService, TmdbService>();

    return services;
  }
}
