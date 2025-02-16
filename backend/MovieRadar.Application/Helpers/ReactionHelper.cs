using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Helpers
{
    public class ReactionHelper
    {
        public static (bool, string) IsReactionValid(RatingReaction ratingReaction, IRatingReactionRepository ratingReactionsRepository)
        {
            if (ratingReaction == null || string.IsNullOrWhiteSpace(ratingReaction.Reaction))
                return (false, "Reaction is null");

            string reaction = ratingReaction.Reaction.ToLower().Trim();

            if (reaction != "like" && reaction != "dislike")
                return (false, "Invalid reaction");

            return (true, "Valid reaction");
        }

        public static async Task<bool> IsRatingReactionUnique(RatingReaction ratingReaction, IRatingReactionRepository ratingReactionsRepository)
        {
            var existingReactions = await ratingReactionsRepository.GetAllByRatingId(ratingReaction.RatingId);
            return !existingReactions.Any(r => r.UserId == ratingReaction.UserId);
        }
    }
}
