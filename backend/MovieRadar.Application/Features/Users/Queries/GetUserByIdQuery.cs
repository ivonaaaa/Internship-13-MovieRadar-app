using MediatR;
using MovieRadar.Domain.Entities;

public record GetUserByIdQuery(int id) : IRequest<User?>;
