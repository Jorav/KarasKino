using KarasKino.Core.Interfaces;
using KarasKino.Core.Users;

namespace KarasKino.Application.Authentication.Register;

public class RegisterHandler(IRepository<User> users) : ICommandHandler<RegisterCommand, Result<Guid>>
{
  public async ValueTask<Result<Guid>> Handle(RegisterCommand cmd, CancellationToken ct)
  {
    var spec = new UserByEmailSpecification(cmd.Email);
    var existing = await users.FirstOrDefaultAsync(spec, ct);

    if (existing is not null)
    {
      if (existing.HasLocalLogin)
        return Result.Conflict();

      existing.SetPassword(BCrypt.Net.BCrypt.HashPassword(cmd.Password));
      await users.SaveChangesAsync(ct);
      return Result.Success(existing.Id);
    }

    var user = new User(cmd.Email);
    user.SetPassword(BCrypt.Net.BCrypt.HashPassword(cmd.Password));
    await users.AddAsync(user, ct);
    await users.SaveChangesAsync(ct);

    return Result.Success(user.Id);
  }
}
