using MediatR;
using MovieRadar.Domain.Interfaces;
using MovieRadar.Application.Features.Users.Commands;



namespace MovieRadar.Application.Features.Users.Handlers
{
    public class CheckUserAuthDataHandler : IRequestHandler<CheckUserAuthDataCommand, bool>
    {
        private readonly IUserRepository userRepository;

        public CheckUserAuthDataHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(CheckUserAuthDataCommand request, CancellationToken cancellationToken)
        {
            var isValidAuthData= await userRepository.CheckAuthData(request.Email, request.Password);
            return isValidAuthData;
        }
    }
}
