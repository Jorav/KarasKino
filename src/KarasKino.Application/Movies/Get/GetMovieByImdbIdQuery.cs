namespace KarasKino.Application.Movies.Get;

public record GetMovieByImdbIdQuery(string ImdbId) : IQuery<Result<MovieResult>>;

public record MovieResult(
  Guid Id,
  string Title,
  string ImdbId,
  string? Description,
  string? PosterUrl,
  string? Director,
  string? ReleaseYear,
  int? Runtime,
  List<string> Genres,
  bool WatchedByKara,
  bool WatchedByJohan);
