using KarasKino.Core.Interfaces;
using KarasKino.Core.Movies;

namespace KarasKino.Application.Movies.Post;

public class PostMovieHandler(IRepository<Movie> movies)
    : ICommandHandler<PostMovieCommand, Result<Guid>>
{
  public async ValueTask<Result<Guid>> Handle(PostMovieCommand cmd, CancellationToken ct)
  {
    var existing = await movies.FirstOrDefaultAsync(new MovieByImdbIdSpecification(cmd.ImdbId), ct);
    if (existing is not null)
      await movies.DeleteAsync(existing, ct);

    var movie = new Movie(
      cmd.Title,
      cmd.ImdbId,
      cmd.Description,
      cmd.PosterUrl,
      cmd.Director,
      cmd.ReleaseYear,
      cmd.Runtime,
      cmd.Genres);

    movie.MarkAsWatched(cmd.WatchedByKara, cmd.WatchedByJohan);

    await movies.AddAsync(movie, ct);
    await movies.SaveChangesAsync(ct);

    return Result.Success(movie.Id);
  }
}
