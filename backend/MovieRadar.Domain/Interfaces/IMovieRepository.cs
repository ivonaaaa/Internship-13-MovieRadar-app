using MovieRadar.Domain.Entities;

namespace MovieRadar.Domain.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetFiltered(string filter, string value);
        Task<IEnumerable<Movie>> OrderByRating(string orderDirection);
    }
}
