using KarasKino.Core.Interfaces;
using KarasKino.Core.Users;

namespace KarasKino.Application.Authentication.Google;

public class GoogleCallbackHandler(IRepository<User> users, IJwtService jwtService)
  : ICommandHandler<GoogleCallbackCommand, Result<string>>
{
  public async ValueTask<Result<string>> Handle(GoogleCallbackCommand cmd, CancellationToken ct)
  {
    var spec = new UserByEmailSpecification(cmd.Email);
    var user = await users.FirstOrDefaultAsync(spec, ct);

    if (user is null)
    {
      user = new User(cmd.Email);
      await users.AddAsync(user, ct);
      await users.SaveChangesAsync(ct);
    }

    var token = jwtService.GenerateToken(user);
    return Result.Success(token);
  }
}
