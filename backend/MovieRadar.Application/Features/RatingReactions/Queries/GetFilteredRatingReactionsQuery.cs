using MediatR;
using MovieRadar.Domain.Entities;

public record GetFilteredRatingReactionsQuery(string filter, string value) : IRequest<IEnumerable<RatingReaction>>;
