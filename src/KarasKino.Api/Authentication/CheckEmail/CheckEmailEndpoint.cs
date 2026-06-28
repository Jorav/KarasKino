using KarasKino.Application.Authentication.ChechEmail;
using KarasKino.Application.Authentication.CheckEmail;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KarasKino.Api.Authentication.CheckEmail;

public class CheckEmailEndpoint(IMediator mediator)
  : Endpoint<CheckEmailRequest, Results<Ok<CheckEmailResponse>, ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(CheckEmailRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Check email login options";
      s.Description = "Returns what login options are available for a given email";
      s.Responses[200] = "Email check result";
    });
    Tags("Authentication");
  }

  public override async Task<Results<Ok<CheckEmailResponse>, ProblemHttpResult>>
    ExecuteAsync(CheckEmailRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(new CheckEmailQuery(req.Email), ct);

    return TypedResults.Ok(new CheckEmailResponse(
      result.Value.Exists,
      result.Value.HasPassword));
  }
}
