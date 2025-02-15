using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;
using MovieRadar.Application.Interfaces;
using MovieRadar.Application.Helpers;

namespace MovieRadar.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository ratingRepository;
        private readonly IMovieRepository movieRepository;
        public RatingService(IRatingRepository ratingRepository, IMovieRepository movieRepository)
        {
            this.ratingRepository = ratingRepository;
            this.movieRepository=movieRepository;
        }

        public async Task<IEnumerable<Rating>> GetAll()
        {
            try
            {
                return await ratingRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all ratings: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<Rating?> GetById(int id)
        {
            try
            {
                return await ratingRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting rating by id: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<int> Add(Rating rating)
        {
            try
            {
                return await ratingRepository.Add(rating);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new rating: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<bool> Update(Rating rating)
        {
            var updateRatingValidation = await RatingHelper.IsRatingValid(rating, movieRepository);
            if (!updateRatingValidation.Item1)
                throw new ArgumentException(updateRatingValidation.Item2);

            try
            {
                return await ratingRepository.Update(rating);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating rating: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                return await ratingRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting rating: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}
