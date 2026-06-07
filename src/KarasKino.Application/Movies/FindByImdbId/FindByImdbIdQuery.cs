using Mediator;

namespace KarasKino.Application.Movies.FindByImdbId;

public record FindByImdbIdQuery(string ImdbId) : IQuery<Result<MovieLookupResult>>;
