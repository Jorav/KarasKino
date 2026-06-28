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

  [JsonPropertyName("imdb_id")]
  public string? ImdbId { get; set; }

  private string? _rawReleaseDate;

  [JsonPropertyName("release_date")]
  public string? RawReleaseDate
  {
    get => _rawReleaseDate;
    set => _rawReleaseDate = value;
  }

  [JsonIgnore]
  public DateTime? ReleaseDate =>
    DateTime.TryParse(_rawReleaseDate, out var parsedDate) ? parsedDate : null;

  [JsonPropertyName("runtime")]
  public int? Runtime { get; set; }

  [JsonPropertyName("genres")]
  public List<TmdbGenreDto> Genres { get; set; } = [];

  [JsonPropertyName("credits")]
  public TmdbCreditsDto Credits { get; set; } = new();
}