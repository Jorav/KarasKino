namespace KarasKino.Api.Movies.Delete;

public class DeleteMovie(IMediator mediator)
  : Endpoint<DeleteMovieRequest, Results<NoContent, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Delete(DeleteMovieRequest.Route);
    Roles(UserRole.Editor.ToString(), UserRole.Admin.ToString());
    Summary(s =>
    {
      s.Summary = "Delete a movie";
      s.Description = "Removes a movie from the database by IMDB ID";
      s.Responses[204] = "Movie deleted";
      s.Responses[404] = "Movie not found";
    });
    Tags("Movies");
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(DeleteMovieRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(new DeleteMovieCommand(req.ImdbId), ct);

    if (result.IsNotFound())
      return TypedResults.NotFound();

    return TypedResults.NoContent();
  }
}