using MediatR;
using MovieRadar.Domain.Entities;

public record GetAllRatingReactionsQuery() : IRequest<IEnumerable<RatingReaction>>;
