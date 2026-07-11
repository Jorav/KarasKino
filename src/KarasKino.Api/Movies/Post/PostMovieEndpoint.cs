using KarasKino.Application.Movies.Post;
using Microsoft.AspNetCore.Http.HttpResults;
using KarasKino.Core.Users;

namespace KarasKino.Api.Movies.Post;

public class AddMovie(IMediator mediator)
  : Endpoint<PostMovieRequest,
             Results<Created<PostMovieResponse>,
                     ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(PostMovieRequest.Route);
    Roles(UserRole.Editor.ToString(), UserRole.Admin.ToString());
    Summary(s =>
    {
      s.Summary = "Add a movie";
      s.Description = "Saves a new movie to the database";
      s.ExampleRequest = new PostMovieRequest
      {
        ImdbId = "tt0111161",
        Title = "The Shawshank Redemption"
      };
      s.Responses[201] = "Movie created";
    });
    Tags("Movies");
  }

  public override async Task<Results<Created<PostMovieResponse>, ProblemHttpResult>>
    ExecuteAsync(PostMovieRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(new PostMovieCommand(
      req.Title,
      req.ImdbId,
      req.Description,
      req.PosterUrl,
      req.Director,
      req.ReleaseYear,
      req.Runtime,
      req.Genres,
      req.WatchedByKara,
      req.WatchedByJohan), ct);

    return TypedResults.Created($"/movies/{result.Value}", new PostMovieResponse(result.Value));
  }
}
