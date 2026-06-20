namespace KarasKino.Api.Movies.SearchTmdb;

public record SearchTmdbMoviesResponse(List<MovieSearchResultResponse> Results);

public record MovieSearchResultResponse(
  string ImdbId,
  string Title,
  string? Director,
  string? ReleaseYear,
  string? PosterUrl);