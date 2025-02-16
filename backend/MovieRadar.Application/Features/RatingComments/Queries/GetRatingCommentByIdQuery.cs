using MediatR;
using MovieRadar.Domain.Entities;

public record GetRatingCommentByIdQuery(int id) : IRequest<RatingComment?>;
