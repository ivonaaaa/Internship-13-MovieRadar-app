using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Helpers
{
    public class RatingHelper
    {
        public static async Task<(bool, string)> IsRatingValid(Rating newRating, IMovieRepository movieRepository)
        {
            if (newRating == null)
                return (false, "The rating is null!");

            var invalidFields = new List<string>();

            var gradeValidation = CheckGrade(newRating.Grade);
            if(!gradeValidation.Item1)
                invalidFields.Add(gradeValidation.Item2);

            var movieIdValidation = await MovieHelper.CheckMovieId(newRating.MovieId, movieRepository);
            if (!movieIdValidation.Item1)
                invalidFields.Add(movieIdValidation.Item2);

            return invalidFields.Count() > 0 ? (false, string.Join("\n", invalidFields)) : (true, "Rating is valid");
        }
        public static (bool, string) CheckGrade(float ratingGrade)
        {
            if (ratingGrade < 1 || ratingGrade > 10)
                return (false, "Grade should be in interval (1 - 10)");

            return (true, "Grade is valid");
        }
    }
}
