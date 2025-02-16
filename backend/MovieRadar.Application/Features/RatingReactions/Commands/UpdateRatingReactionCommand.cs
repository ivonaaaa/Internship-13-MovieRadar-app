using MediatR;
using MovieRadar.Domain.Entities;

public record UpdateRatingReactionCommand(RatingReaction reaction) : IRequest<bool>;
