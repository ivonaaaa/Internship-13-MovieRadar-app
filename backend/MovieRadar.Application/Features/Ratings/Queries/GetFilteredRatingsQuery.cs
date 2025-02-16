using MediatR;
using MovieRadar.Domain.Entities;

public record GetFilteredRatingsQuery(string filter, string parameter) : IRequest<IEnumerable<Rating>>;
