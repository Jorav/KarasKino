using KarasKino.Core.Movies.Model;

namespace KarasKino.Core.Movies.Interfaces;

public interface ITmdbService
{
  Task<MovieLookupResult?> FindByImdbId(string imdbId, CancellationToken ct);
}
