using System.Text.Json.Serialization;

namespace KarasKino.Infrastructure.Movies.Model;

public class TmdbCrewMemberDto
{
  [JsonPropertyName("job")]
  public string Job { get; set; } = string.Empty;

  [JsonPropertyName("name")]
  public string Name { get; set; } = string.Empty;
}
