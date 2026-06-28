using Ardalis.Specification;
using KarasKino.Core.Users;

namespace KarasKino.Application.Authentication;

public sealed class UserByEmailSpecification : SingleResultSpecification<User>
{
  public UserByEmailSpecification(string email) =>
    Query.Where(u => u.Email == email.ToLowerInvariant());
}
