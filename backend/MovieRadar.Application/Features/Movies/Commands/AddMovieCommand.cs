using MediatR;
using MovieRadar.Domain.Entities;

public record AddMovieCommand(Movie movie) : IRequest<int>;
