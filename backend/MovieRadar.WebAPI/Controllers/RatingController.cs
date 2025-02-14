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
        public async Task<ActionResult<IEnumerable<Rating>>> GetAllComments()
        {
            var allRatings = await ratingService.GetAll();
            return Ok(allRatings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetCommentById(int id)
        {
            var rating = await ratingService.GetById(id);
            if(rating == null)
                return NotFound();

            return Ok(rating);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddComment([FromBody] Rating newRating)
        {
            var id = await ratingService.Add(newRating);
            return CreatedAtAction(nameof(GetCommentById), new { id }, newRating);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment([FromBody] Rating updatedRating, int id)
        {
            if(id != updatedRating.Id)
                return BadRequest();

            var updated = await ratingService.Update(updatedRating);
            return updated ? NoContent() : NotFound();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var deleted = await ratingService.DeleteById(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
