using KarasKino.Core.Base;

namespace KarasKino.Core.Movies;

public class Movie : Entity
{
  private Movie() { }

  public Movie(
    string title,
    string imdbId,
    string? description,
    string? posterPath,
    string? director,
    string? releaseYear,
    int? runtime,
    List<string> genres)
  {
    Title = Guard.Against.NullOrWhiteSpace(title, nameof(title));
    ImdbId = Guard.Against.NullOrWhiteSpace(imdbId, nameof(imdbId));

    Description = description;
    PosterPath = posterPath;
    Director = director;
    ReleaseYear = releaseYear;
    Runtime = runtime;
    Genres = genres ?? [];
  }

  public string Title { get; private set; } = string.Empty;
  public string ImdbId { get; private set; } = string.Empty;
  public string? Description { get; private set; }
  public string? PosterPath { get; private set; }
  public string? Director { get; private set; }
  public string? ReleaseYear { get; private set; }
  public int? Runtime { get; private set; }
  public List<string> Genres { get; private set; } = [];
  public bool WatchedByKara { get; private set; }
  public bool WatchedByJohan { get; private set; }

  public void MarkAsWatched(bool watchedByKara, bool watchedByJohan)
  {
    WatchedByKara = watchedByKara;
    WatchedByJohan = watchedByJohan;
  }
}
