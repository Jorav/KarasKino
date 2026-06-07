namespace KarasKino.Core.Movies.Model;

public record TmdbMovieResult(
  string Title,
  string? Description,
  string? PosterUrl,
  string? Director,
  string? ReleaseDate,
  int? Runtime,
  string? ImdbId,
  List<string> Genres
);
