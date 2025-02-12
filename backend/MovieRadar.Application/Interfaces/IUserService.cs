using MovieRadar.Domain.Entities;

namespace MovieRadar.Application.Services
{
    public interface IUserService : IService<User>
    {
        Task<User?> GetByEmail(string email);
    }
}
