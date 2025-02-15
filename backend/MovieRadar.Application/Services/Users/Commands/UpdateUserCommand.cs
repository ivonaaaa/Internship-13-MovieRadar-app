using MediatR;
using MovieRadar.Domain.Entities;

public record UpdateUserCommand(User user) : IRequest<bool>;
