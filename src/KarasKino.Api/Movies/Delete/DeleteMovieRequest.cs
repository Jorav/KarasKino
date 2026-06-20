namespace KarasKino.Api.Movies.Delete;

public class DeleteMovieRequest
{
  public const string Route = "/movies/{ImdbId}";
  public string ImdbId { get; set; } = string.Empty;
}