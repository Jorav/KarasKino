namespace KarasKino.UseCases.Movies.FindByImdbId;

public record FindByImdbIdQuery(string ImdbId) : IQuery<Result<MovieLookupResult>>;
