using KarasKino.Application.Contributors;
using KarasKino.Core.ContributorAggregate;

namespace KarasKino.Application.Contributors.Get;

public record GetContributorQuery(ContributorId ContributorId) : IQuery<Result<ContributorDto>>;
