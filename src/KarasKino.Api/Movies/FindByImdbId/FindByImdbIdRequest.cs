namespace KarasKino.Api.Movies.FindByImdbId;

public class FindByImdbIdRequest
{
  public const string Route = "/movies/search";
  public string ImdbId { get; set; } = string.Empty;
}
