using MediatR;
using MovieRadar.Domain.Entities;

public record AddRatingCommand(Rating rating) : IRequest<int>;
