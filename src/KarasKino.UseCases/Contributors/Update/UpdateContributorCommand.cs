using KarasKino.Core.ContributorAggregate;

namespace KarasKino.UseCases.Contributors.Update;

public record UpdateContributorCommand(ContributorId ContributorId, ContributorName NewName) : ICommand<Result<ContributorDto>>;
