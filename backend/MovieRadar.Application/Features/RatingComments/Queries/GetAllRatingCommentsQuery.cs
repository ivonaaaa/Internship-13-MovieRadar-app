using MediatR;
using MovieRadar.Domain.Entities;

public record GetAllRatingCommentsQuery() : IRequest<IEnumerable<RatingComment>>;
