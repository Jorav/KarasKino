using KarasKino.UseCases.Movies.FindByImdbId;

namespace KarasKino.Core.Movies.Interfaces;

public interface ITmdbService
{
  Task<MovieLookupResult?> FindByImdbId(string imdbId, CancellationToken ct);
}
