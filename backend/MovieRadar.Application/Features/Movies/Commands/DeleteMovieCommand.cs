using MediatR;

public record DeleteMovieCommand(int id) : IRequest<bool>;
