using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace UserRadar.Application.Services.Users.Handlers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
    {
        private readonly IUserRepository userRepository;

        public GetAllUsersHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await userRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all users: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}