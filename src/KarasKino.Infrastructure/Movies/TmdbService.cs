using KarasKino.Core.Movies.Interfaces;
using KarasKino.Core.Movies.Model;
using Microsoft.Extensions.Options;

namespace KarasKino.Infrastructure.Movies;

public class TmdbService(TmdbClient tmdbClient, IOptions<TmdbConfiguration> config) : ITmdbService
{
  public async Task<TmdbMovieResult?> FindByImdbId(string imdbId, CancellationToken ct)
  {
    var response = await tmdbClient.FindByImdbId(imdbId, ct);
    if (response == null || response.MovieResults.Count == 0)
      return null;

    var movie = response.MovieResults[0];
    var posterUrl = movie.PosterPath != null
      ? $"{config.Value.ImageBaseUrl}{movie.PosterPath}"
      : null;
    var year = movie.ReleaseDate?.Length >= 4
      ? int.Parse(movie.ReleaseDate[..4])
      : (int?)null;

    return new TmdbMovieResult(
      movie.Title,
      movie.Overview,
      posterUrl,
      null,
      year,
      imdbId);
  }
}
