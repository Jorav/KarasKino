namespace KarasKino.Application.Movies.SearchTmdb;

public record SearchTmdbMoviesQuery(string Query) : IQuery<Result<List<MovieSearchResult>>>;