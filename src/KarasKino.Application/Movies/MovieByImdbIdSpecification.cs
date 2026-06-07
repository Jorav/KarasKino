using Ardalis.Specification;
using KarasKino.Core.Movies;

namespace KarasKino.Application.Movies;

public sealed class MovieByImdbIdSpecification : SingleResultSpecification<Movie>
{
  public MovieByImdbIdSpecification(string imdbId) 
    => Query.Where(m => m.ImdbId == imdbId);
}
