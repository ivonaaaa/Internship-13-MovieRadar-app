using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRadar.Application.Interfaces;
using MovieRadar.Domain.Entities;

namespace MovieRadar.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService ratingService;
        public RatingController(IRatingService ratingService)
        {
            this.ratingService = ratingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> GetAllRatings()
        {
            var allRatings = await ratingService.GetAll();
            return Ok(allRatings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetRatingById(int id)
        {
            var rating = await ratingService.GetById(id);
            if(rating == null)
                return NotFound();

            return Ok(rating);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddRating([FromBody] Rating newRating)
        {
            var id = await ratingService.Add(newRating);
            return CreatedAtAction(nameof(GetRatingById), new { id }, newRating);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRating([FromBody] Rating updatedRating, int id)
        {
            if(id != updatedRating.Id)
                return BadRequest("Not matching id");

            try
            {
                var updated = await ratingService.Update(updatedRating);
                return updated ? NoContent() : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating new rating: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var deleted = await ratingService.DeleteById(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
