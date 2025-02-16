using MediatR;
using MovieRadar.Domain.Entities;

public record UpdateRatingCommentCommand(RatingComment comment) : IRequest<bool>;
