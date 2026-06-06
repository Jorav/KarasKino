using KarasKino.Core.ContributorAggregate;

namespace KarasKino.Application.Contributors.Delete;

public record DeleteContributorCommand(ContributorId ContributorId) : ICommand<Result>;
