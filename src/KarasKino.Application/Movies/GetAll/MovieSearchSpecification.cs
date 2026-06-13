using Ardalis.Specification;
using KarasKino.Core.Movies;

namespace KarasKino.Application.Movies.GetAll;

internal class MovieSearchSpecification : Specification<Movie>
{
  public MovieSearchSpecification(int page, int pageSize, string? search)
  {
    if (!string.IsNullOrWhiteSpace(search))
      Query.Where(m => m.Title.ToLower().Contains(search.ToLower()));

    if (string.IsNullOrWhiteSpace(search))
      Query.OrderBy(m => m.Title);
    else
      Query.OrderByDescending(m => m.Title.ToLower().StartsWith(search.ToLower()))
           .ThenBy(m => m.Title);

    Query.Skip((page - 1) * pageSize).Take(pageSize);
  }
}
