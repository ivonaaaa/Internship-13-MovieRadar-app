using Microsoft.AspNetCore.Mvc;
using MovieRadar.Application.Interfaces;
using MovieRadar.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        public async Task<ActionResult<IEnumerable<RatingComment>>> GetAllRatingComments()
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
        public async Task<ActionResult<RatingComment>> GetRatingCommentById(int id)
        {
            var ratingComment = await ratingCommentService.GetById(id);
            if (ratingComment == null)
                return NotFound();

            return Ok(ratingComment);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddRatingComment([FromBody] RatingComment newRatingsComments)
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
        public async Task<IActionResult> UpdateRatingComment([FromBody] RatingComment ratingsComments, int id)
        {
            if (id != ratingsComments.Id)
                return BadRequest("Not matching id");

            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
                return Unauthorized();

            if (userId != ratingsComments.UserId)
                return Forbid();

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
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
                return Unauthorized();

            var ratingCommentToDelete = await ratingCommentService.GetById(id);

            if(ratingCommentToDelete == null)
                return NotFound();
            
            if (userId != ratingCommentToDelete.UserId)
                return Forbid();
            
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
