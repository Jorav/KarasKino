using KarasKino.Core.ContributorAggregate;

namespace KarasKino.UseCases.Contributors.Delete;

public record DeleteContributorCommand(ContributorId ContributorId) : ICommand<Result>;
