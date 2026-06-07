using KarasKino.Application.Movies.FindByImdbId;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KarasKino.Api.Movies.FindByImdbId;

public class FindByImdbId(IMediator mediator)
  : Endpoint<FindByImdbIdRequest,
             Results<Ok<FindByImdbIdResponse>,
                     NotFound,
                     ProblemHttpResult>>
{
  public override void Configure()
  {
    Get(FindByImdbIdRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Find a movie by IMDB ID";
      s.Description = "Looks up movie details from TMDB using an IMDB ID (e.g. tt0111161)";
      s.ExampleRequest = new FindByImdbIdRequest { ImdbId = "tt0111161" };
      s.Responses[200] = "Movie found";
      s.Responses[404] = "Movie not found";
    });
    Tags("Movies");
  }

  public override async Task<Results<Ok<FindByImdbIdResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(FindByImdbIdRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(new FindByImdbIdQuery(req.ImdbId), ct);

    if (result.IsNotFound())
      return TypedResults.NotFound();

    return TypedResults.Ok(new FindByImdbIdResponse(
      result.Value.Title,
      result.Value.Description,
      result.Value.PosterUrl,
      result.Value.Director,
      result.Value.ReleaseDate,
      result.Value.Runtime,
      result.Value.ImdbId,
      result.Value.Genres));
  }
}
