using System.Text.Json.Serialization;

namespace KarasKino.Infrastructure.Movies;

public class TmdbFindResponse
{
  [JsonPropertyName("movie_results")]
  public List<TmdbMovieDto> MovieResults { get; set; } = [];
}
