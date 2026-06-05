using KarasKino.Core.ContributorAggregate;

namespace KarasKino.UseCases.Contributors;
public record ContributorDto(ContributorId Id, ContributorName Name, PhoneNumber PhoneNumber);
