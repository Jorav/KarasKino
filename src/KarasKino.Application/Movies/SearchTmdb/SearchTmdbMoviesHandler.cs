namespace KarasKino.Application.Movies.SearchTmdb;

public class SearchTmdbMoviesHandler(ITmdbService tmdbService)
  : IQueryHandler<SearchTmdbMoviesQuery, Result<List<MovieSearchResult>>>
{
  public async ValueTask<Result<List<MovieSearchResult>>> Handle(SearchTmdbMoviesQuery query, CancellationToken ct)
  {
    var results = await tmdbService.SearchMovies(query.Query, ct);
    return Result.Success(results);
  }
}