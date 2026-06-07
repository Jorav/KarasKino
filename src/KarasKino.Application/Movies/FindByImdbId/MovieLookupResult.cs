namespace KarasKino.Application.Movies.FindByImdbId;

public record MovieLookupResult(
  string Title,
  string? Description,
  string? PosterUrl,
  string? Director,
  string? ReleaseDate,
  int? Runtime,
  string? ImdbId,
  List<string> Genres
);
