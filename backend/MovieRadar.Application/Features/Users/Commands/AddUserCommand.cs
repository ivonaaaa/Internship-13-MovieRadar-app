using MediatR;
using MovieRadar.Domain.Entities;

public record AddUserCommand(User user) : IRequest<int>;
