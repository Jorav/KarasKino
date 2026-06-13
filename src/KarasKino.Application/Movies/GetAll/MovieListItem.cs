namespace KarasKino.Application.Movies.GetAll;

public record MovieListItem(
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
