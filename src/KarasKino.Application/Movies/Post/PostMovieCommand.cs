using Mediator;

namespace KarasKino.Application.Movies.Post;

public record PostMovieCommand(
  string Title,
  string ImdbId,
  string? Description,
  string? PosterUrl,
  string? Director,
  string? ReleaseYear,
  int? Runtime,
  List<string> Genres,
  bool WatchedByKara,
  bool WatchedByJohan) : ICommand<Result<Guid>>;
