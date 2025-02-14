using MovieRadar.Domain.Entities;

namespace MovieRadar.Application.Helpers
{
    public class RatingHelper
    {
        public static (bool, string) isRatingValid(Rating rating)
        {
            var uniqueUserMovieRating = isUnique(rating.MovieId, rating.UserId);

            return (true, "Rating is valid");
        }
        public static (bool, string) isUnique(int movieId, int userId)
        {
            //var allRatings = 
            return (true, "Rating is unique");
        }
    }
}
