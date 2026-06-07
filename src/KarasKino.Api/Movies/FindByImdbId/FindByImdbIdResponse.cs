namespace KarasKino.Api.Movies.FindByImdbId;

public record FindByImdbIdResponse(
  string Title,
  string? Description,
  string? PosterUrl,
  string? Director,
  string? ReleaseDate,
  int? Runtime,
  string? ImdbId,
  List<string> Genres
);
