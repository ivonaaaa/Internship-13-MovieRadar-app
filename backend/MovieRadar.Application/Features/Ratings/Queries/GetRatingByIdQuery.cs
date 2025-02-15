using MediatR;
using MovieRadar.Domain.Entities;

public record GetRatingByIdQuery(int id) : IRequest<Rating?>;
