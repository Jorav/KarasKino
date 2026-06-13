namespace KarasKino.Api.Movies.GetAll;

public class GetMoviesRequest
{
  public const string Route = "/movies";
  public int Page { get; set; } = 1;
  public int PageSize { get; set; } = 24;
  public string? Search { get; set; }
}
