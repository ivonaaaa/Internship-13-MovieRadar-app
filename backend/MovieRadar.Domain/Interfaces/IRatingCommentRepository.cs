using MovieRadar.Domain.Entities;

namespace MovieRadar.Domain.Interfaces
{
    public interface IRatingCommentRepository : IRepository<RatingComment>
    {
        Task<IEnumerable<RatingComment>> GetAllByRatingId(int id);
    }
}
