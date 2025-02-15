using Microsoft.AspNetCore.Mvc;
using MovieRadar.Application.Interfaces;
using MovieRadar.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace MovieRadar.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RatingCommentsController : ControllerBase
    {
        private readonly IRatingCommentService ratingCommentService;
        public RatingCommentsController(IRatingCommentService ratingCommentService)
        {
            this.ratingCommentService = ratingCommentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatingsComments>>> GetAllRatingComments()
        {
            try
            {
                var allRatingComments = await ratingCommentService.GetAll();
                return Ok(allRatingComments);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Error getting all rating comments: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RatingsComments>> GetRatingCommentById(int id)
        {
            var ratingComment = await ratingCommentService.GetById(id);
            if (ratingComment == null)
                return NotFound();

            return Ok(ratingComment);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddRatingComment([FromBody] RatingsComments newRatingsComments)
        {
            try
            {
                var newRatingCommentId = await ratingCommentService.Add(newRatingsComments);
                return CreatedAtAction(nameof(GetRatingCommentById), new { id = newRatingCommentId }, newRatingsComments);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding new rating comment: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRatingComment([FromBody] RatingsComments ratingsComments, int id)
        {
            Console.WriteLine(ratingsComments.Id + $" {ratingsComments.RatingId } {ratingsComments.Comment} {ratingsComments.UserId}" + id);

            if (id != ratingsComments.Id)
                return BadRequest("Not matching id");

            try
            {
                var updated = await ratingCommentService.Update(ratingsComments);
                return updated ? NoContent() : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating rating comment: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRatingComment(int id)
        {
            try
            {
                var deleted = await ratingCommentService.DeleteById(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting   rating comment: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }
    }
}
