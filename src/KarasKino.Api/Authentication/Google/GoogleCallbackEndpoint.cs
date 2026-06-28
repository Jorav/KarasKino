using KarasKino.Application.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace KarasKino.Api.Authentication.Google;

public class GoogleCallbackEndpoint(IMediator mediator, IConfiguration config)
  : EndpointWithoutRequest<Results<RedirectHttpResult, UnauthorizedHttpResult, ProblemHttpResult>>
{
  public override void Configure()
  {
    Get("/authentication/google/callback");
    AllowAnonymous();
    Tags("Authentication");
  }

  public override async Task<Results<RedirectHttpResult, UnauthorizedHttpResult, ProblemHttpResult>>
    ExecuteAsync(CancellationToken ct)
  {
    var authResult = await HttpContext.AuthenticateAsync("External");
    if (!authResult.Succeeded)
      return TypedResults.Unauthorized();

    var email = authResult.Principal?.FindFirstValue(ClaimTypes.Email);
    if (string.IsNullOrEmpty(email))
      return TypedResults.Unauthorized();

    var result = await mediator.Send(new GoogleCallbackCommand(email), ct);
    if (result.IsUnauthorized())
      return TypedResults.Unauthorized();

    HttpContext.Response.Cookies.Append("auth", result.Value, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.None,
      Expires = DateTimeOffset.UtcNow.AddHours(2),
      Path = "/"
    });
    var frontendUrl = config["FrontendUrl"];
    return TypedResults.Redirect(frontendUrl);
  }
}
