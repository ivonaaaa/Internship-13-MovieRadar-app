using MediatR;
using MovieRadar.Domain.Entities;

public record GetFilteredRatingCommentsQuery(string filter, string parameter) : IRequest<IEnumerable<RatingComment>>;
