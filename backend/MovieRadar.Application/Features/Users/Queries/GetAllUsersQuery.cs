using MediatR;
using MovieRadar.Domain.Entities;

public record GetAllUsersQuery() : IRequest<IEnumerable<User>>;
