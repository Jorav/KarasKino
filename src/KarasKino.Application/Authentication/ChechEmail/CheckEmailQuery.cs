namespace KarasKino.Application.Authentication.ChechEmail;

public record CheckEmailQuery(string Email) : IQuery<Result<CheckEmailResult>>;
