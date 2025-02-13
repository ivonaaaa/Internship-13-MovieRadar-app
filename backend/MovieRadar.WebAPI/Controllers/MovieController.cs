using Microsoft.AspNetCore.Mvc;
using MovieRadar.Application.Interfaces;
using MovieRadar.Domain.Entities;

namespace MovieRadar.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService movieService;
        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies([FromQuery] string? filter, [FromQuery] string? value)
        {
            var validFilters = new HashSet<string> { "release_year", "genre" };

            if (string.IsNullOrEmpty(filter) && string.IsNullOrEmpty(value))
            {
                var allMovies = await movieService.GetAll();
                return (allMovies == null || !allMovies.Any()) ? NotFound() : Ok(allMovies);
            }


            if (!validFilters.Contains(filter))
                return BadRequest($"Invalid filter: '{filter}'");

            if(string.IsNullOrWhiteSpace(value))
                return BadRequest("Value cannot be empty");

            var filteredMovies = await movieService.GetFilteredMovies(filter, value);
    
            return (filteredMovies == null || !filteredMovies.Any())? NotFound() : Ok(filteredMovies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovieById(int id)
        {
            var movie = await movieService.GetById(id);
            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        [HttpGet("order")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetOrderedMoviesByRatingGrade([FromQuery] string orderDirection)
        {
            if(orderDirection != "asc" && orderDirection != "desc")
                return BadRequest();

            var movies = await movieService.GetOrderedMoviesByGrade(orderDirection);
            return (movies == null || !movies.Any()) ? NotFound() : Ok(movies);
        }

        [HttpPost]
        public async Task<ActionResult> AddMovie([FromBody] Movie newMovie)
        {
            var newMovieId = await movieService.Add(newMovie);
            return CreatedAtAction(nameof(GetMovieById), new { id = newMovieId }, newMovie);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateMovie([FromBody] Movie updatedMovie, int id)
        {
            if(id != updatedMovie.Id)
                return BadRequest();

            var updated = await movieService.Update(updatedMovie);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var deleted = await movieService.DeleteById(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
