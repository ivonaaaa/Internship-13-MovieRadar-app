using MediatR;
using MovieRadar.Domain.Entities;

public record UpdateMovieCommand(Movie Movie) : IRequest<bool>;
