using MediatR;
using MovieRadar.Domain.Entities;

public record AddRatingReactionCommand(RatingReaction reaction) : IRequest<int>;
