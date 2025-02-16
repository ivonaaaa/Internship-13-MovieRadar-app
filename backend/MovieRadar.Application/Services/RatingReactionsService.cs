using MovieRadar.Application.Helpers;
using MovieRadar.Application.Interfaces;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Services
{
    public class RatingReactionsService : IRatingReactionsService
    {
        private readonly IRatingReactionsRepository ratingReactionsRepository;
        public RatingReactionsService(IRatingReactionsRepository ratingReactionsRepository)
        {
            this.ratingReactionsRepository = ratingReactionsRepository;
        }

        public async Task<IEnumerable<RatingsReactions>> GetAll()
        {
            try
            {
                return await ratingReactionsRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all reactions: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<RatingsReactions?> GetById(int id)
        {
            try
            {
                return await ratingReactionsRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting reaction by id: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<int> Add(RatingsReactions ratingReactions)
        {
            var reactionValidation = await ReactionHelper.IsReactionValid(ratingReactions, ratingReactionsRepository, isAdd: true);
            if(!reactionValidation.Item1)
                throw new ArgumentException(reactionValidation.Item2);

            try
            {
                return await ratingReactionsRepository.Add(ratingReactions);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while adding rating reaction: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<bool> Update(RatingsReactions ratingReactions)
        {
            var reactionValidation = await ReactionHelper.IsReactionValid(ratingReactions, ratingReactionsRepository, isAdd: false);
            if (!reactionValidation.Item1)
                throw new ArgumentException(reactionValidation.Item2);

            try
            {
                return await ratingReactionsRepository.Update(ratingReactions);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating reaction: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                return await ratingReactionsRepository.Delete(id);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error while deleting reaction: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<IEnumerable<RatingsReactions>> GetAllReactionsByRatingId(int id)
        {
            try
            {
                return await ratingReactionsRepository.GetAllByRatingId(id);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error while getting all reactions by rating id: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}
