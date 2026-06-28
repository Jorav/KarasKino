namespace KarasKino.Application.Movies.Delete;

public record DeleteMovieCommand(string ImdbId) : ICommand<Result>;