using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KarasKino.Api.Authentication;

public class MeEndpoint : EndpointWithoutRequest<Results<Ok<MeResponse>, UnauthorizedHttpResult>>
{
  public override void Configure()
  {
    Get("/authentication/me");
    Summary(s =>
    {
      s.Summary = "Get current user";
      s.Responses[200] = "Current user info";
      s.Responses[401] = "Not authenticated";
    });
    Tags("Authentication");
  }

  public override Task<Results<Ok<MeResponse>, UnauthorizedHttpResult>> ExecuteAsync(CancellationToken ct)
  {
    var email = User.FindFirstValue(ClaimTypes.Email);
    var role = User.FindFirstValue(ClaimTypes.Role);

    if (email is null)
      return Task.FromResult<Results<Ok<MeResponse>, UnauthorizedHttpResult>>(TypedResults.Unauthorized());

    return Task.FromResult<Results<Ok<MeResponse>, UnauthorizedHttpResult>>(
      TypedResults.Ok(new MeResponse(email, role ?? "Viewer")));
  }
}

public record MeResponse(string Email, string Role);
