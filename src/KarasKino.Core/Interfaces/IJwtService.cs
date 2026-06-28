using KarasKino.Core.Users;

namespace KarasKino.Core.Interfaces;

public interface IJwtService
{
  string GenerateToken(User user);
}
