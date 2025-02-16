using MediatR;
using MovieRadar.Domain.Entities;

public record GetRatingReactionByIdQuery(int id) : IRequest<RatingReaction?>;
