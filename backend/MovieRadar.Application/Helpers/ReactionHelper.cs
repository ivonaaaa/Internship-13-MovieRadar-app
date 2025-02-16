using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Helpers
{
    public class ReactionHelper
    {
        public static async Task<(bool, string)> IsReactionValid(RatingsReactions ratingReaction, IRatingReactionsRepository ratingReactionsRepository, bool isAdd)
        {
            if (ratingReaction == null || string.IsNullOrWhiteSpace(ratingReaction.Reaction))
                return (false, "Reaction is null");

            string reaction = ratingReaction.Reaction?.ToLower()?.Trim();

            if (reaction != "like" && reaction != "dislike")
                return (false, "Invalid reaction");

            if (isAdd && !await CheckIsUnique(ratingReaction, ratingReactionsRepository))
                return (false, "User has already reacted to this rating");
            

            return (true, "Valid reaction");
        }
        private static async Task<bool> CheckIsUnique(RatingsReactions ratingReaction, IRatingReactionsRepository ratingReactionsRepository)
        {
            var existingReactions = await ratingReactionsRepository.GetAllByRatingId(ratingReaction.RatingId);
            return !existingReactions.Any(r => r.UserId == ratingReaction.UserId);
        }
    }
}
