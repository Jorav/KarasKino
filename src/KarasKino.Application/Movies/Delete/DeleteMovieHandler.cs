namespace KarasKino.Application.Movies.Delete;

public class DeleteMovieHandler(IRepository<Movie> movies)
  : ICommandHandler<DeleteMovieCommand, Result>
{
  public async ValueTask<Result> Handle(DeleteMovieCommand cmd, CancellationToken ct)
  {
    var spec = new MovieByImdbIdSpecification(cmd.ImdbId);
    var movie = await movies.FirstOrDefaultAsync(spec, ct);

    if (movie is null)
      return Result.NotFound();

    await movies.DeleteAsync(movie, ct);
    await movies.SaveChangesAsync(ct);

    return Result.Success();
  }
}