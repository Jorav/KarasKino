namespace KarasKino.Infrastructure.Movies;

public class TmdbConfiguration
{
  public const string SectionName = "Tmdb";
  public string AccessToken { get; set; } = string.Empty;
  public string BaseUrl { get; set; } = string.Empty;
  public string ImageBaseUrl { get; set; } = string.Empty;
}
