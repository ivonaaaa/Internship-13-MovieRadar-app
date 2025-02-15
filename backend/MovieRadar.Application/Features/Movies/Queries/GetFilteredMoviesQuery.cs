using MediatR;
using MovieRadar.Domain.Entities;

public record GetFilteredMoviesQuery(string filter, string parameter) : IRequest<IEnumerable<Movie>>;
