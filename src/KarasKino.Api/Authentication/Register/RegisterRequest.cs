namespace KarasKino.Api.Auth.Register;

public class RegisterRequest
{
  public const string Route = "/authentication/register";
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
}
