using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.Users.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User?>
    {
        private readonly IUserRepository userRepository;

        public GetUserByIdHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await userRepository.GetById(request.id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by id: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}