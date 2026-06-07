using System.Text.Json.Serialization;

namespace KarasKino.Infrastructure.Movies.Model;

public class TmdbMovieDetailsResponse
{
  [JsonPropertyName("title")]
  public string Title { get; set; } = string.Empty;

  [JsonPropertyName("overview")]
  public string? Overview { get; set; }

  [JsonPropertyName("poster_path")]
  public string? PosterPath { get; set; }

  [JsonPropertyName("release_date")]
  public string? ReleaseDate { get; set; }

  [JsonPropertyName("runtime")]
  public int? Runtime { get; set; }

  [JsonPropertyName("genres")]
  public List<TmdbGenreDto> Genres { get; set; } = [];

  [JsonPropertyName("credits")]
  public TmdbCreditsDto Credits { get; set; } = new();
}
