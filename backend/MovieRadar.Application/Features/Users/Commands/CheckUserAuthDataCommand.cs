using MediatR;

namespace MovieRadar.Application.Features.Users.Commands
{
    public record CheckUserAuthDataCommand(string Email, string Password) : IRequest<bool>;
  
}
