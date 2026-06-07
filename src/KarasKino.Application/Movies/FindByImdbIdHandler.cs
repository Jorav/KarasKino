using KarasKino.Core.Movies.Interfaces;
using KarasKino.UseCases.Movies.FindByImdbId;

namespace KarasKino.Application.Movies.FindByImdbId;

public class FindByImdbIdHandler(ITmdbService tmdbService)
{
  public async Task<Result<MovieLookupResult>> Handle(
    FindByImdbIdQuery query,
    CancellationToken ct)
  {
    var result = await tmdbService.FindByImdbId(query.ImdbId, ct);

    if (result == null)
      return Result.NotFound();

    return Result.Success(new MovieLookupResult(
      result.Title,
      result.Description,
      result.PosterUrl,
      result.Director,
      result.ReleaseDate,
      result.Runtime,
      result.ImdbId,
      result.Genres));
  }
}
