using KarasKino.Core.ContributorAggregate;

namespace KarasKino.Application.Contributors;

public record ContributorDto(ContributorId Id, ContributorName Name, PhoneNumber PhoneNumber);
