using MovieRadar.Application.Interfaces;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Services
{
    public class RatingsCommentsService : IRatingCommentService
    {
        private readonly IRatingCommentsRepository ratingCommentsRepository;
        public RatingsCommentsService(IRatingCommentsRepository ratingCommentsRepository)
        {
            this.ratingCommentsRepository = ratingCommentsRepository;
        }
        public async Task<IEnumerable<RatingsComments>> GetAll()
        {
            try
            {
                return await ratingCommentsRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all movies: {ex.Message}, inner: {ex.InnerException}");
            }
        }
        public async Task<RatingsComments> GetById(int id)
        {
            try
            {
                return await ratingCommentsRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting movie by id: {ex.Message}, inner: {ex.InnerException}");
            }
        }
        public async Task<int> Add(RatingsComments ratingsComments)
        {
            try
            {
                return await ratingCommentsRepository.Add(ratingsComments);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new movie: {ex.Message}, inner: {ex.InnerException}");
            }
        }
        public async Task<bool> Update(RatingsComments ratingsComments)
        {

            try
            {
                return await ratingCommentsRepository.Update(ratingsComments);
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
                return await ratingCommentsRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting movie: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}
