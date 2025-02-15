using MovieRadar.Application.Services;
using MovieRadar.Domain.Entities;

namespace MovieRadar.Application.Interfaces
{
    public interface IRatingService : IService<Rating>
    {
        Task<(int, int)> GetLikesDislikes(int ratingId);
        Task<bool> RemoveLikeDislike(int reactionId);
    }
}
