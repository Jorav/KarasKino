using Ardalis.Result;
using KarasKino.Core.Movies.Interfaces;

namespace KarasKino.UseCases.Movies.FindByImdbId;

public class FindByImdbIdHandler(ITmdbService tmdbService)
  : IQueryHandler<FindByImdbIdQuery, Result<MovieLookupResult>>
{
  public async ValueTask<Result<MovieLookupResult>> Handle(
    FindByImdbIdQuery query,
    CancellationToken cancellationToken)
  {
    var result = await tmdbService.FindByImdbId(query.ImdbId, cancellationToken);

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
