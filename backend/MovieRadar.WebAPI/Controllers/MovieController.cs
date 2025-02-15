using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRadar.Application.Interfaces;
using MovieRadar.Domain.Entities;
using MovieRadar.Application.Helpers;

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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddMovie([FromBody] Movie newMovie)
        {
            try 
            {
                var newMovieId = await movieService.Add(newMovie);
                return CreatedAtAction(nameof(GetMovieById), new { id = newMovieId }, newMovie);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding new movie: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie([FromBody] Movie updatedMovie, int id)
        {
            if (id != updatedMovie.Id)
                return BadRequest("Not matching id");

            try
            {
                var updated = await movieService.Update(updatedMovie);
                return updated ? NoContent() : NotFound();
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error updating new movie: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var deleted = await movieService.DeleteById(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
