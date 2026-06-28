using KarasKino.Core.Interfaces;
using KarasKino.Core.Users;

namespace KarasKino.Application.Authentication.Login;

public class LoginHandler(IRepository<User> users, IJwtService jwtService)
  : ICommandHandler<LoginCommand, Result<string>>
{
  public async ValueTask<Result<string>> Handle(LoginCommand cmd, CancellationToken ct)
  {
    var spec = new UserByEmailSpecification(cmd.Email);
    var user = await users.FirstOrDefaultAsync(spec, ct);

    if (user is null || user.PasswordHash is null)
      return Result.Unauthorized();

    if (!BCrypt.Net.BCrypt.Verify(cmd.Password, user.PasswordHash))
      return Result.Unauthorized();

    var token = jwtService.GenerateToken(user);
    return Result.Success(token);
  }
}
