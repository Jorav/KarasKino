using KarasKino.Application.Movies.FindByImdbId;
using KarasKino.Application.Movies.SearchTmdb;

namespace KarasKino.Infrastructure.Movies;

public class TmdbService(TmdbClient tmdbClient, IOptions<TmdbConfiguration> config) : ITmdbService
{
  private readonly TmdbConfiguration _config = config.Value;

  public async Task<MovieLookupResult?> FindByImdbId(string imdbId, CancellationToken ct)
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

    return new MovieLookupResult(
      details.Title,
      details.Overview,
      posterUrl,
      director,
      details.ReleaseDate?.Year.ToString(),
      details.Runtime,
      imdbId,
      genres);
  }

  public async Task<List<MovieSearchResult>> SearchMovies(string query, CancellationToken ct)
  {
    var searchResponse = await tmdbClient.SearchMovies(query, ct);
    if (searchResponse == null || searchResponse.Results.Count == 0)
      return [];

    var topResults = searchResponse.Results
      .OrderByDescending(r => r.Popularity)
      .Take(4)
      .ToList();

    var detailTasks = topResults.Select(async result =>
    {
      var details = await tmdbClient.GetMovieDetails(result.Id, ct);
      if (details == null)
        return null;

      var director = details.Credits.Crew
        .FirstOrDefault(c => c.Job == "Director")?.Name;

      var posterUrl = details.PosterPath != null
        ? $"{_config.ImageBaseUrl}{details.PosterPath}"
        : null;

      return new MovieSearchResult(
        details.ImdbId,
        details.Title,
        director,
        details.ReleaseDate?.Year.ToString(),
        posterUrl);
    });

    var results = await Task.WhenAll(detailTasks);
    return results.Where(r => r is not null && !string.IsNullOrEmpty(r.ImdbId)).ToList()!;
  }
}
