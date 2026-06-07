using KarasKino.Core.Movies.Interfaces;
using KarasKino.Core.Movies.Model;
using Microsoft.Extensions.Options;

namespace KarasKino.Infrastructure.Movies;

public class TmdbService(TmdbClient tmdbClient, IOptions<TmdbConfiguration> config) : ITmdbService
{
  private readonly TmdbConfiguration _config = config.Value;

  public async Task<TmdbMovieResult?> FindByImdbId(string imdbId, CancellationToken ct = default)
  {
    var findResponse = await tmdbClient.FindByImdbId(imdbId, ct);
    if (findResponse == null || findResponse.MovieResults.Count == 0)
      return null;

    var tmdbId = findResponse.MovieResults[0].Id;

    var details = await tmdbClient.GetMovieDetails(tmdbId, ct);
    if (details == null)
      return null;

    var posterUrl = details.PosterPath != null
      ? $"{_config.ImageBaseUrl}{details.PosterPath}"
      : null;

    var director = details.Credits.Crew
      .FirstOrDefault(c => c.Job == "Director")?.Name;

    var genres = details.Genres
      .Select(g => g.Name)
      .ToList();

    return new TmdbMovieResult(
      details.Title,
      details.Overview,
      posterUrl,
      director,
      details.ReleaseDate,
      details.Runtime,
      imdbId,
      genres);
  }
}
