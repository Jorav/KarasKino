using KarasKino.Application.Movies.Get;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KarasKino.Api.Movies.Get;

public class GetMovieByImdbId(IMediator mediator)
  : Endpoint<GetMovieByImdbIdRequest,
             Results<Ok<GetMovieByImdbIdResponse>,
                     NotFound,
                     ProblemHttpResult>>
{
  public override void Configure()
  {
    Get(GetMovieByImdbIdRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Get a movie by IMDB ID";
      s.Description = "Retrieves a saved movie from the database by IMDB ID";
      s.Responses[200] = "Movie found";
      s.Responses[404] = "Movie not found in database";
    });
    Tags("Movies");
  }

  public override async Task<Results<Ok<GetMovieByImdbIdResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetMovieByImdbIdRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(new GetMovieByImdbIdQuery(req.ImdbId), ct);

    if (result.IsNotFound())
      return TypedResults.NotFound();

    return TypedResults.Ok(new GetMovieByImdbIdResponse(
      result.Value.Id,
      result.Value.Title,
      result.Value.ImdbId,
      result.Value.Description,
      result.Value.PosterUrl,
      result.Value.Director,
      result.Value.ReleaseYear,
      result.Value.Runtime,
      result.Value.Genres,
      result.Value.WatchedByKara,
      result.Value.WatchedByJohan));
  }
}
