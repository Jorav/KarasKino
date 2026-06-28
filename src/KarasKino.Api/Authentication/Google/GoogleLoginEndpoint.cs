using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

namespace KarasKino.Api.Authentication.Google;

public class GoogleLoginEndpoint : EndpointWithoutRequest
{
  public override void Configure()
  {
    Get("/authentication/google");
    AllowAnonymous();
    Tags("Authentication");
  }

  public override async Task<object?> ExecuteAsync(CancellationToken ct)
  {
    await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
    {
      RedirectUri = "/api/authentication/google/callback"
    });
    await HttpContext.Response.CompleteAsync();
    return null;
  }
}
