public class TmdbSearchResult
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string? PosterPath { get; set; }
  public string? ReleaseDate { get; set; }
  public double Popularity { get; set; }
}