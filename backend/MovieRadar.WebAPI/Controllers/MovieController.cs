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
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies([FromQuery] string? filter, [FromQuery] string? parameter)
        {
            IEnumerable<Movie> movies;

            if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(parameter))
                movies = await movieService.GetFilteredMovies(filter, parameter);
            else
                movies = await movieService.GetAll();
                
            return (movies == null || !movies.Any())? NotFound() : Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovieById(int id)
        {
            var movie = await movieService.GetById(id);
            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult> AddMovie([FromBody] Movie newMovie)
        {
            var newMovieId = await movieService.Add(newMovie);
            return CreatedAtAction(nameof(GetMovieById), new { id = newMovieId }, newMovie);
        }

        [HttpPut("{id}")]
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
