using System.Text.Json.Serialization;

namespace KarasKino.Infrastructure.Movies.Model;

public class TmdbFindResponse
{
  [JsonPropertyName("movie_results")]
  public List<TmdbFindDto> MovieResults { get; set; } = [];
}
