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
            var allComments = await ratingService.GetAll();
            return Ok(allComments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetCommentById(int id)
        {
            var comment = await ratingService.GetById(id);
            if(comment == null)
                return NotFound();

            return Ok(comment);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddComment([FromBody] Rating newComment)
        {
            var id = await ratingService.Add(newComment);
            return CreatedAtAction(nameof(GetCommentById), new { id }, newComment);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment([FromBody] Rating updatedComment, int id)
        {
            if(id != updatedComment.Id)
                return BadRequest();

            var updated = await ratingService.Update(updatedComment);
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
