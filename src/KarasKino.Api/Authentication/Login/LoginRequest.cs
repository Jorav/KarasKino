namespace KarasKino.Api.Authentication.Login;

public class LoginRequest
{
  public const string Route = "/authentication/login";
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
}
