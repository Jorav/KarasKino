using System.Text.Json.Serialization;

namespace KarasKino.Infrastructure.Movies.Model;

public class TmdbCreditsDto
{
  [JsonPropertyName("crew")]
  public List<TmdbCrewMemberDto> Crew { get; set; } = [];
}
