using MovieRadar.Domain.Entities;

namespace MovieRadar.Domain.Interfaces
{
    public interface IRatingReactionRepository : IRepository<RatingReaction>
    {
        Task<IEnumerable<RatingReaction>> GetFiltered(string filter, string value);
    }
}
