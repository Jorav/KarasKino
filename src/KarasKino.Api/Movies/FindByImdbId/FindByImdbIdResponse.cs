namespace KarasKino.Api.Movies.FindByImdbId;

public record FindByImdbIdResponse(
  string Title,
  string? Description,
  string? PosterUrl,
  string? Director,
  int? Year,
  string? ImdbId
);
