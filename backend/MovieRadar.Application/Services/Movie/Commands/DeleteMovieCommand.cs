using MediatR;

public record DeleteMovieCommand(int Id) : IRequest<bool>;
