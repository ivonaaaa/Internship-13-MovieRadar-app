using MediatR;
using MovieRadar.Domain.Entities;

public record AddRatingCommentCommand(RatingComment comment) : IRequest<int>;
