using MovieRadar.Domain.Entities;

namespace MovieRadar.Domain.Interfaces
{
    public interface IRatingRepository : IRepository<Rating>
    {
        Task<(int, int)> GetLikesAndDislikes(int id);
        Task<bool> RemoveLikeDislike(int userId);
    }
}
