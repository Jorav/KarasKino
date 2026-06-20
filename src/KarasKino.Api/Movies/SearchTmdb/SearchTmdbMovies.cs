namespace KarasKino.Api.Movies.SearchTmdb;

public class SearchTmdbMovies(IMediator mediator)
  : Endpoint<SearchTmdbMoviesRequest, Results<Ok<SearchTmdbMoviesResponse>, ProblemHttpResult>>
{
  public override void Configure()
  {
    Get(SearchTmdbMoviesRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Search movies by title";
      s.Description = "Searches TMDB for movies matching the given title, returns top 5 by relevance";
    });
    Tags("Movies");
  }

  public override async Task<Results<Ok<SearchTmdbMoviesResponse>, ProblemHttpResult>>
    ExecuteAsync(SearchTmdbMoviesRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(new SearchTmdbMoviesQuery(req.Query), ct);

    return TypedResults.Ok(new SearchTmdbMoviesResponse(
      result.Value.Select(r => new MovieSearchResultResponse(
        r.ImdbId, r.Title, r.Director, r.ReleaseYear, r.PosterUrl)).ToList()));
  }
}