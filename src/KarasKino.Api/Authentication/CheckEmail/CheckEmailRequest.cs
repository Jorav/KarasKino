namespace KarasKino.Api.Authentication.CheckEmail;

public class CheckEmailRequest
{
  public const string Route = "/authentication/check-email";
  public string Email { get; set; } = string.Empty;
}
