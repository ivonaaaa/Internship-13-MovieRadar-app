using MovieRadar.Application.Helpers;
using MovieRadar.Application.Interfaces;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Services
{
    public class MovieService 
    {
        private readonly IMovieRepository movieRepository;
        public MovieService(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }
        public async Task<IEnumerable<Movie>> GetAll()
        {
            try
            {
                return await movieRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all movies: {ex.Message}, inner: {ex.InnerException}");
            }
        }
        public async Task<Movie?> GetById(int id)
        {
            try
            {
                return await movieRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting movie by id: {ex.Message}, inner: {ex.InnerException}");
            }
        }
        public async Task<int> Add(Movie movie)
        {
            var newMovieValidation = MovieHelper.IsMovieValid(movie);
            if (!newMovieValidation.Item1)
                throw new ArgumentException(newMovieValidation.Item2);
         
            try
            {
                return await movieRepository.Add(movie);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new movie: {ex.Message}, inner: {ex.InnerException}");
            }
        }
        public async Task<bool> Update(Movie movie)
        {
            var updateMovieValidation = MovieHelper.IsMovieValid(movie);
            if (!updateMovieValidation.Item1)
                throw new ArgumentException(updateMovieValidation.Item2);

            try
            {
                return await movieRepository.Update(movie);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating movie: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                return await movieRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting movie: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<IEnumerable<Movie>> GetFilteredMovies(string filter, string parameter)
        {
            try
            {
                return await movieRepository.GetFilteredMovies(filter, parameter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting filtered movies: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<IEnumerable<Movie>> GetOrderedMoviesByGrade(string orderDirection)
        {
            try
            {
                return await movieRepository.OrderByRating(orderDirection);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting ordered movies: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}
