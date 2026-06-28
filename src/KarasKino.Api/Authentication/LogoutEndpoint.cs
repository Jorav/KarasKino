using Microsoft.AspNetCore.Http.HttpResults;

namespace KarasKino.Api.Authentication.Logout;

public class LogoutEndpoint : EndpointWithoutRequest<Ok>
{
  public override void Configure()
  {
    Post("/authentication/logout");
    AllowAnonymous();
    Tags("Authentication");
  }

  public override Task<Ok> ExecuteAsync(CancellationToken ct)
  {
    HttpContext.Response.Cookies.Delete("auth", new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.None,
      Path = "/"
    });

    return Task.FromResult(TypedResults.Ok());
  }
}
