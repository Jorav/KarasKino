using Ardalis.Result;
using KarasKino.Application.Movies.GetAll;
using KarasKino.Core.Interfaces;
using KarasKino.Core.Movies;
using Mediator;

namespace KarasKino.Application.Movies.GetAll;

public class GetMoviesHandler(IRepository<Movie> movies)
  : IQueryHandler<GetMoviesQuery, Result<PagedResult<MovieListItem>>>
{
  public async ValueTask<Result<PagedResult<MovieListItem>>> Handle(GetMoviesQuery query, CancellationToken ct)
  {
    var spec = new MovieSearchSpecification(query.Page, query.PageSize, query.Search);
    var countSpec = new MovieSearchCountSpecification(query.Search);

    var items = await movies.ListAsync(spec, ct);
    var total = await movies.CountAsync(countSpec, ct);

    var result = new PagedResult<MovieListItem>(
      items.Select(m => new MovieListItem(
        m.Id,
        m.Title,
        m.ImdbId,
        m.Description,
        m.PosterPath,
        m.Director,
        m.ReleaseYear,
        m.Runtime,
        m.Genres,
        m.WatchedByKara,
        m.WatchedByJohan)).ToList(),
      total,
      query.Page,
      query.PageSize);

    return Result.Success(result);
  }
}
