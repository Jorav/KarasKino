using System.Text.Json.Serialization;

namespace KarasKino.Infrastructure.Movies;

public class TmdbMovieDto
{
  [JsonPropertyName("title")]
  public string Title { get; set; } = string.Empty;

  [JsonPropertyName("overview")]
  public string? Overview { get; set; }

  [JsonPropertyName("poster_path")]
  public string? PosterPath { get; set; }

  [JsonPropertyName("release_date")]
  public string? ReleaseDate { get; set; }
}
