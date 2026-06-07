using KarasKino.Application.Movies.Post;
using KarasKino.Core.Interfaces;
using KarasKino.Core.Movies;

namespace KarasKino.Application.Movies.Get;

public class GetMovieByImdbIdHandler(IRepository<Movie> movies)
  : IQueryHandler<GetMovieByImdbIdQuery, Result<MovieResult>>
{
  public async ValueTask<Result<MovieResult>> Handle(GetMovieByImdbIdQuery query, CancellationToken ct)
  {
    var movie = await movies.FirstOrDefaultAsync(new MovieByImdbIdSpecification(query.ImdbId), ct);

    if (movie is null)
      return Result.NotFound();

    return Result.Success(new MovieResult(
      movie.Id,
      movie.Title,
      movie.ImdbId,
      movie.Description,
      movie.PosterPath,
      movie.Director,
      movie.ReleaseYear,
      movie.Runtime,
      movie.Genres,
      movie.WatchedByKara,
      movie.WatchedByJohan));
  }
}
