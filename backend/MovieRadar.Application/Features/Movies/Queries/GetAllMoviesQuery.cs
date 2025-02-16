using MediatR;
using MovieRadar.Domain.Entities;

public record GetAllMoviesQuery() : IRequest<IEnumerable<Movie>>;
