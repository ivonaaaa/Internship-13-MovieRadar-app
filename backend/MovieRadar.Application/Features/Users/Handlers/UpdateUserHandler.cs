using MediatR;
using MovieRadar.Application.Helpers;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.Users.Handlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserRepository userRepository;

        public UpdateUserHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var updateUserValidation = UserHelper.IsUserValid(request.user);
            if (!updateUserValidation.Item1)
                throw new ArgumentException(updateUserValidation.Item2);

            try
            {
                return await userRepository.Update(request.user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }

}