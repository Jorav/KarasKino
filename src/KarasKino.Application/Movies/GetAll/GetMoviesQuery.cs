namespace KarasKino.Application.Movies.GetAll;

public record GetMoviesQuery(
  int Page,
  int PageSize,
  string? Search) : IQuery<Result<PagedResult<MovieListItem>>>;
