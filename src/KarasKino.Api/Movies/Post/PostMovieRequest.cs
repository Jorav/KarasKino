namespace KarasKino.Api.Movies.Post;

public class PostMovieRequest
{
  public const string Route = "/movies";

  public string ImdbId { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public string? Description { get; set; }
  public string? PosterUrl { get; set; }
  public string? Director { get; set; }
  public string? ReleaseYear { get; set; }
  public int? Runtime { get; set; }
  public List<string> Genres { get; set; } = [];
  public bool WatchedByKara { get; set; }
  public bool WatchedByJohan { get; set; }
}
