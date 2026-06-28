using KarasKino.Application.Authentication.Login;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KarasKino.Api.Authentication.Login;

public class LoginEndpoint(IMediator mediator, IHttpContextAccessor httpContext)
  : Endpoint<LoginRequest, Results<Ok, UnauthorizedHttpResult, ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(LoginRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Login with email and password";
      s.Responses[200] = "Logged in successfully";
      s.Responses[401] = "Invalid credentials";
    });
    Tags("Authentication");
  }

  public override async Task<Results<Ok, UnauthorizedHttpResult, ProblemHttpResult>>
    ExecuteAsync(LoginRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(new LoginCommand(req.Email, req.Password), ct);

    if (result.IsUnauthorized())
      return TypedResults.Unauthorized();

    httpContext.HttpContext!.Response.Cookies.Append("auth", result.Value, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.None,
      Expires = DateTimeOffset.UtcNow.AddHours(1),
      Path = "/"
    });

    return TypedResults.Ok();
  }
}
