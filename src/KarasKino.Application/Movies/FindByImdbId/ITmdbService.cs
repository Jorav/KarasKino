namespace KarasKino.Application.Movies.FindByImdbId;

public interface ITmdbService
{
  Task<MovieLookupResult?> FindByImdbId(string imdbId, CancellationToken ct);
  Task<List<MovieSearchResult>> SearchMovies(string query, CancellationToken ct = default);
}
