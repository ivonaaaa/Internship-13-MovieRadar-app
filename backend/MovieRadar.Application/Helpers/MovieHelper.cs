using MovieRadar.Domain.Entities;

namespace MovieRadar.Application.Helpers
{
    public class MovieHelper
    {
        public static (bool, string) IsMovieValid(Movie newMovie)
        {
            var unValidFields = new List<string>();

            var titleValidation = CheckTitle(newMovie.Title);
            if(!titleValidation.Item1)
                unValidFields.Add(titleValidation.Item2);

            var genreValidation = CheckGenre(newMovie.Genre);
            if (!genreValidation.Item1)
                unValidFields.Add(genreValidation.Item2);

            var releaseYearValidation = CheckReleaseYear(newMovie.ReleaseYear);
            if(!releaseYearValidation.Item1)
                unValidFields.Add(releaseYearValidation.Item2);

            return unValidFields.Count() > 0 ? (false, string.Join("\n", unValidFields)) : (true, "Movie is valid");
        }
        public static (bool, string) CheckTitle(string movieTitle)
        {
            return !string.IsNullOrWhiteSpace(movieTitle) ? (true, "Title is valid") : (false, "Title cannot be empty");
        }
        public static (bool, string) CheckGenre(string movieGenre)
        {
            return !string.IsNullOrWhiteSpace(movieGenre) ? (true, "Genre is valid") : (false, "Genre cannot be empty");
        }
        public static (bool, string) CheckReleaseYear(int movieReleaseYear)
        {
            return (movieReleaseYear > 1850 && movieReleaseYear <= DateTime.Now.Year) ? (true, "Release year is valid") : (false, "Release year is not valid");
        }
    }
}
