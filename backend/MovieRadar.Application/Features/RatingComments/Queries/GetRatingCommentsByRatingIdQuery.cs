using MediatR;
using MovieRadar.Domain.Entities;

public record GetRatingCommentsByRatingIdQuery(int id) : IRequest<IEnumerable<RatingComment>>;
