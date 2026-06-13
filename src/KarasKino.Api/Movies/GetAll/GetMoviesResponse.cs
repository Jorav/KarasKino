namespace KarasKino.Api.Movies.GetAll;

public record GetMoviesResponse(
  List<MovieListItemResponse> Items,
  int TotalCount,
  int Page,
  int PageSize);

public record MovieListItemResponse(
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
