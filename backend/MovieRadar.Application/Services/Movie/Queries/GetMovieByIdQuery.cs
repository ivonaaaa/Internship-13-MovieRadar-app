using MediatR;
using MovieRadar.Domain.Entities;

public record GetMovieByIdQuery(int Id) : IRequest<Movie>;
