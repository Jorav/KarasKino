using System.Text.Json.Serialization;

namespace KarasKino.Infrastructure.Movies.Model;

public class TmdbFindDto
{
  [JsonPropertyName("id")]
  public int Id { get; set; }
}
