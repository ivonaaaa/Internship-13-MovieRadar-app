using MovieRadar.Application.Services;
using MovieRadar.Domain.Entities;

namespace MovieRadar.Application.Interfaces
{
    public interface IMovieService : IService<Movie>
    {
        Task<IEnumerable<Movie>> GetFilteredMovies(string filter, string parameter);
        Task<IEnumerable<Movie>> GetOrderedMoviesByGrade(string orderDirection);
    }
}
