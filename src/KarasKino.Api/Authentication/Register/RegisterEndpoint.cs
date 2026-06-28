using KarasKino.Api.Auth.Register;
using KarasKino.Application.Authentication.Register;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KarasKino.Api.Authentication.Register;

public class RegisterEndpoint(IMediator mediator)
  : Endpoint<RegisterRequest, Results<Ok, Conflict, ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(RegisterRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Register a new user";
      s.Responses[200] = "Registered successfully";
      s.Responses[409] = "Email already exists";
    });
    Tags("Authentication");
  }

  public override async Task<Results<Ok, Conflict, ProblemHttpResult>>
    ExecuteAsync(RegisterRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(new RegisterCommand(req.Email, req.Password), ct);

    if (result.IsConflict())
      return TypedResults.Conflict();

    return TypedResults.Ok();
  }
}
