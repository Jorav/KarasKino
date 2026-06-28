namespace KarasKino.Application.Authentication.Login;

public record LoginCommand(string Email, string Password) : ICommand<Result<string>>;
