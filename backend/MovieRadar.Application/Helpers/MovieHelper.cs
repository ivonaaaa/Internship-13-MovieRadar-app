using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieRadar.Domain.Entities;

namespace MovieRadar.Application.Helpers
{
    public class MovieHelper
    {
        public static (bool, string) IsMovieValid(Movie newMovie)
        {
            string message = string.Empty;

            if (newMovie.Title == null)
            {
                message = "Title cannot be null";
                return (false, message); 
            }

            if(newMovie.Genre == null)
            {
                message = "Genre cannot be null";
                return (false, message);
            }

            return (true, "User is valid");
        }
        public static (bool, string) CheckTitle(string movies)
        {

        }
    }
}
