using KarasKino.Core.ContributorAggregate;

namespace KarasKino.UseCases.Contributors.Get;

public record GetContributorQuery(ContributorId ContributorId) : IQuery<Result<ContributorDto>>;
