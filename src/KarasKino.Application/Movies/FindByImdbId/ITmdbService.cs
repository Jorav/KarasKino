namespace KarasKino.Application.Movies.FindByImdbId;

public interface ITmdbService
{
  Task<MovieLookupResult?> FindByImdbId(string imdbId, CancellationToken ct);
}
