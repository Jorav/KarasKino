using Ardalis.Specification;
using KarasKino.Core.Movies;

namespace KarasKino.Application.Movies.GetAll;

public sealed class MovieSearchCountSpecification : Specification<Movie>
{
  public MovieSearchCountSpecification(string? search)
  {
    if (!string.IsNullOrWhiteSpace(search))
      Query.Where(m => m.Title.ToLower().Contains(search.ToLower()));
  }
}
