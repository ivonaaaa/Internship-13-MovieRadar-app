using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
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
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            var allMovies = await movieService.GetAll();
            return Ok(allMovies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovieByIf(int id)
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
            return CreatedAtAction(nameof(GetMovieByIf), new { id = newMovieId }, newMovie);
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
