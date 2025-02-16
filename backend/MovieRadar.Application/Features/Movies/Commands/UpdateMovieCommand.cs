using MediatR;
using MovieRadar.Domain.Entities;

public record UpdateMovieCommand(Movie movie) : IRequest<bool>;
