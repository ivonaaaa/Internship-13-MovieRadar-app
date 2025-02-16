using MediatR;
using MovieRadar.Domain.Entities;

public record GetAllRatingsQuery() : IRequest<IEnumerable<Rating>>;
