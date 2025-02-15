using MediatR;
using MovieRadar.Domain.Entities;

public record GetOrderedMoviesByGradeQuery(string orderDirection) : IRequest<IEnumerable<Movie>>;
