using MediatR;
using MovieRadar.Application.Helpers;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.Users.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, int>
    {
        private readonly IUserRepository userRepository;

        public AddUserHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<int> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var updateUserValidation = UserHelper.IsUserValid(request.user);
            if (!updateUserValidation.Item1)
                throw new ArgumentException(updateUserValidation.Item2);

            var user = await userRepository.GetByEmail(request.user.Email);
            if (user != null)
                throw new ArgumentException("Email is already taken!");

            try
            {
                return await userRepository.Add(request.user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new user: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }

}