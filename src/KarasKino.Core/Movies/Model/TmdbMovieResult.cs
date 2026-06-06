namespace KarasKino.Core.Movies.Model;

public record TmdbMovieResult(
  string Title,
  string? Description,
  string? PosterUrl,
  string? Director,
  int? Year,
  string? ImdbId
);
