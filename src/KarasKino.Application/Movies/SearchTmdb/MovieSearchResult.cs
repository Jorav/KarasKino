namespace KarasKino.Application.Movies.SearchTmdb;

public record MovieSearchResult(
  string ImdbId,
  string Title,
  string? Director,
  string? ReleaseYear,
  string? PosterUrl);