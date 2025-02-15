using MediatR;
using MovieRadar.Domain.Entities;

public record AddMovieCommand(Movie Movie) : IRequest<int>;
