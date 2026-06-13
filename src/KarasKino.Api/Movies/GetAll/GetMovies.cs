using KarasKino.Application.Movies.GetAll;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KarasKino.Api.Movies.GetAll;

public class GetMovies(IMediator mediator)
  : Endpoint<GetMoviesRequest, Results<Ok<GetMoviesResponse>, ProblemHttpResult>>
{
  public override void Configure()
  {
    Get(GetMoviesRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Get all movies";
      s.Description = "Returns a paged, searchable list of movies sorted alphabetically";
      s.Responses[200] = "Movies returned";
    });
    Tags("Movies");
  }

  public override async Task<Results<Ok<GetMoviesResponse>, ProblemHttpResult>>
    ExecuteAsync(GetMoviesRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(
      new GetMoviesQuery(req.Page, req.PageSize, req.Search), ct);

    return TypedResults.Ok(new GetMoviesResponse(
      result.Value.Items.Select(m => new MovieListItemResponse(
        m.Id,
        m.Title,
        m.ImdbId,
        m.Description,
        m.PosterUrl,
        m.Director,
        m.ReleaseYear,
        m.Runtime,
        m.Genres,
        m.WatchedByKara,
        m.WatchedByJohan)).ToList(),
      result.Value.TotalCount,
      result.Value.Page,
      result.Value.PageSize));
  }
}
