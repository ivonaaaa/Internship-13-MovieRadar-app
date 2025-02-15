using MediatR;
using MovieRadar.Domain.Entities;

public record UpdateRatingCommand(Rating rating) : IRequest<bool>;
