using MovieRadar.Domain.Interfaces;
using MediatR;
using MovieRadar.Application.Commands;
using MovieRadar.Application.Queries;
using MovieRadar.Domain.Entities;

namespace MovieRadar.Application.Handlers
{
    public class UserCommandHandler : IRequestHandler<GetAll, IEnumerable<User>>
    {
        private readonly IUserRepository userRepository;
        public UserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> Handle(GetAll request, CancellationToken cancellationToken)
        {
            try
            {
                return await userRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new user: {ex.Message}, inner: {ex.InnerException}");
            }
        }

    }
}
