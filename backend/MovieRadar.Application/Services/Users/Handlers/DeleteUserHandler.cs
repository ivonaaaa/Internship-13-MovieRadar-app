using MediatR;
using MovieRadar.Domain.Interfaces;

namespace RatingRadar.Application.Services.Ratings.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await userRepository.Delete(request.id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }

}