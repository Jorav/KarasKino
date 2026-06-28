using KarasKino.Core.Base;

namespace KarasKino.Core.Users;

public class User : Entity
{
  private User() { }

  public User(string email, UserRole role = UserRole.Viewer)
  {
    Email = email.ToLowerInvariant();
    Role = role;
  }

  public string Email { get; private set; } = string.Empty;
  public string? PasswordHash { get; private set; }
  public UserRole Role { get; private set; } = UserRole.Viewer;
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

  public bool HasLocalLogin => PasswordHash is not null;
  public void SetPassword(string passwordHash) => PasswordHash = passwordHash;
  public void SetRole(UserRole role) => Role = role;
}
