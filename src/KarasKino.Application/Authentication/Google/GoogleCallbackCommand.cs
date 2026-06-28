namespace KarasKino.Application.Authentication.Google;

public record GoogleCallbackCommand(string Email) : ICommand<Result<string>>;
