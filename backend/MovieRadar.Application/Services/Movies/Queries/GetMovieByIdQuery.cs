using MediatR;
using MovieRadar.Domain.Entities;

public record GetMovieByIdQuery(int id) : IRequest<Movie?>;
