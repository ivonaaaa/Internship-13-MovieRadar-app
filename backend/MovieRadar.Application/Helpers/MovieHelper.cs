using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Helpers
{
    public class MovieHelper
    {
        public static (bool, string) IsMovieValid(Movie newMovie)
        {
            if (newMovie == null)
                return (false, "The movie is null!");

            var invalidFields = new List<string>();

            var titleValidation = CheckTitle(newMovie.Title);
            if(!titleValidation.Item1)
                invalidFields.Add(titleValidation.Item2);

            var genreValidation = CheckGenre(newMovie.Genre);
            if (!genreValidation.Item1)
                invalidFields.Add(genreValidation.Item2);

            var releaseYearValidation = CheckReleaseYear(newMovie.ReleaseYear);
            if(!releaseYearValidation.Item1)
                invalidFields.Add(releaseYearValidation.Item2);

            return invalidFields.Count() > 0 ? (false, string.Join("\n", invalidFields)) : (true, "Movie is valid");
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
        public static async Task<(bool, string)> CheckMovieId(int movieId, IMovieRepository movieRepository)
        {
            return await movieRepository.GetById(movieId) != null ? (true, "The movie ID is valid") : (false, "The movie ID does not exist");
        }
    }
}
