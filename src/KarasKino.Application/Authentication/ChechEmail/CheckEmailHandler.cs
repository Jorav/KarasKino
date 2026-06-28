using KarasKino.Application.Authentication.ChechEmail;
using KarasKino.Core.Interfaces;
using KarasKino.Core.Users;

namespace KarasKino.Application.Authentication.CheckEmail;

public class CheckEmailHandler(IRepository<User> users)
  : IQueryHandler<CheckEmailQuery, Result<CheckEmailResult>>
{
  public async ValueTask<Result<CheckEmailResult>> Handle(CheckEmailQuery query, CancellationToken ct)
  {
    var spec = new UserByEmailSpecification(query.Email);
    var user = await users.FirstOrDefaultAsync(spec, ct);

    if (user is null)
      return Result.Success(new CheckEmailResult(false, false));

    return Result.Success(new CheckEmailResult(true, user.HasLocalLogin));
  }
}
