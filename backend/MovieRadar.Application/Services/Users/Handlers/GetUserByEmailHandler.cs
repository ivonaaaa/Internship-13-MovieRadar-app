using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace UserRadar.Application.Services.Users.Handlers
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, User?>
    {
        private readonly IUserRepository userRepository;

        public GetUserByEmailHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<User?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await userRepository.GetByEmail(request.email);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by email: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}