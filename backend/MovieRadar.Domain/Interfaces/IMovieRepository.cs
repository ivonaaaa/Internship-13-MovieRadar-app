using MovieRadar.Domain.Entities;

namespace MovieRadar.Domain.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetFilteredMovies(string filter, string parameter);
        Task<IEnumerable<Movie>> OrderByRating(string orderDirection);
    }
}
