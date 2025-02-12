using MediatR;
using MovieRadar.Domain.Entities;

namespace MovieRadar.Application.Commands
{
    public class AddUserCommand : IRequest<int>
    {
        public User user { get; set; }
        public AddUserCommand(User user)
        {
            this.user = user;
        }
    }
}
