using MovieRadar.Application.Services;
using MovieRadar.Domain.Entities;


namespace MovieRadar.Application.Interfaces
{
    public interface IRatingReactionsService : IService<RatingsReactions>
    {
        Task<IEnumerable<RatingsReactions>> GetAllReactionsByRatingId(int id);
    }
}
