using MovieRadar.Domain.Entities;

namespace MovieRadar.Domain.Interfaces
{
    public interface IRatingReactionsRepository : IRepository<RatingsReactions>
    {
        Task<IEnumerable<RatingsReactions>> GetAllByRatingId(int id);
    }
}
