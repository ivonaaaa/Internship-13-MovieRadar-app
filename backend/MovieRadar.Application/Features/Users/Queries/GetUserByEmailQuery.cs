using MediatR;
using MovieRadar.Domain.Entities;

public record GetUserByEmailQuery(string email) : IRequest<User?>;
