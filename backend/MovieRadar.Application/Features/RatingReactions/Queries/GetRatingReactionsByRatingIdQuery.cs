using MediatR;
using MovieRadar.Domain.Entities;

public record GetRatingReactionsByRatingIdQuery(int id) : IRequest<IEnumerable<RatingReaction>>;
