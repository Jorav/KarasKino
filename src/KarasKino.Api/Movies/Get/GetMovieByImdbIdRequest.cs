namespace KarasKino.Api.Movies.Get;

public class GetMovieByImdbIdRequest
{
  public const string Route = "/movies/{ImdbId}";
  public string ImdbId { get; set; } = string.Empty;
}
