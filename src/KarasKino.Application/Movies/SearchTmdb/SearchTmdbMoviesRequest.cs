namespace KarasKino.Api.Movies.SearchTmdb;

public class SearchTmdbMoviesRequest
{
  public const string Route = "/movies/search-tmdb";
  public string Query { get; set; } = string.Empty;
}