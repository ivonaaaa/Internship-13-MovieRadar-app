using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;
using MovieRadar.Application.Interfaces;

namespace MovieRadar.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository ratingRepository;
        public RatingService(IRatingRepository ratingRepository)
        {
            this.ratingRepository = ratingRepository;
        }

        public async Task<IEnumerable<Rating>> GetAll()
        {
            try
            {
                return await ratingRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all comments: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<Rating> GetById(int id)
        {
            try
            {
                return await ratingRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting comment by id: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<int> Add(Rating comment)
        {
            try
            {
                return await ratingRepository.Add(comment);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new comment: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<bool> Update(Rating comment)
        {
            try
            {
                return await ratingRepository.Update(comment);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating comment: {ex.Message}, inner: {ex.InnerException}");
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
                throw new Exception($"Error deleting comment: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<(int, int)> GetLikesDislikes(int ratingId)
        {
            try
            {
                return await ratingRepository.GetLikesAndDislikes(ratingId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting likes: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<bool> RemoveLikeDislike(int reactionId)
        {
            try
            {
                return await ratingRepository.RemoveLikeDislike(reactionId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing like/dislike: {ex.Message}, inner: {ex.InnerException}");

            }
        }
    }
}
