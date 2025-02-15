using MovieRadar.Domain.Entities;

namespace MovieRadar.Application.Helpers
{
    public class ReactionHelper
    {
        public static (bool, string) IsReactionValid(RatingsReactions ratingReaction)
        {
            if (ratingReaction == null)
                return (false, "Reaction is null");

            if (string.IsNullOrWhiteSpace(ratingReaction.Reaction))
                return (false, "Reaction is empty or null");

            string reaction = ratingReaction.Reaction?.ToLower()?.Trim();

            if (reaction != "like" && reaction != "dislike")
                return (false, "Invalid reaction");

            return (true, "Valid reaction");
        }
    }
}
