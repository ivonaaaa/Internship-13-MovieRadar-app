using MovieRadar.Domain.Entities;

namespace MovieRadar.Domain.Interfaces
{
    public interface IRatingReactionRepository : IRepository<RatingReaction>
    {
        Task<IEnumerable<RatingReaction>> GetAllByRatingId(int id);
    }
}
