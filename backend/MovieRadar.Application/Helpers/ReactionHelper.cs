using MovieRadar.Domain.Entities;

namespace MovieRadar.Application.Helpers
{
    public class ReactionHelper
    {
        public static (bool, string) IsReactionValid(RatingsReactions ratingReaction)
        {
            if (ratingReaction == null)
                return (false, "Reaction is null");

            if (ratingReaction.Reaction != "Like" && ratingReaction.Reaction != "Dislike")
                return (false, "invalid reaction");

            return (true, "valid reaction");
        }
    }
}
