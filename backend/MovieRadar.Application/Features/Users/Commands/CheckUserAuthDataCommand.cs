using MediatR;
using MovieRadar.Domain.Entities;

namespace MovieRadar.Application.Features.Users.Commands
{
    public record CheckUserAuthDataCommand(string Email, string Password) : IRequest<bool>;
  
}
