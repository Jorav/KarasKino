using KarasKino.Application.Contributors;
using KarasKino.Core.ContributorAggregate;

namespace KarasKino.Application.Contributors.Update;

public record UpdateContributorCommand(ContributorId ContributorId, ContributorName NewName) : ICommand<Result<ContributorDto>>;
