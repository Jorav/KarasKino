namespace KarasKino.Application.Authentication.Register;

public record RegisterCommand(string Email, string Password) : ICommand<Result<Guid>>;
