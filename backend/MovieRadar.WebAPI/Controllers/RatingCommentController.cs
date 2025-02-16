using Microsoft.AspNetCore.Mvc;
using MovieRadar.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MediatR;

namespace MovieRadar.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RatingCommentController : ControllerBase
    {
        private readonly IMediator mediator;
        public RatingCommentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatingComment>>> GetAllRatingComments([FromQuery] string? filter, [FromQuery] string? value)
        {
            var validFilters = new HashSet<string> { "movie_id" };

            if (string.IsNullOrEmpty(filter) && string.IsNullOrEmpty(value))
            {
                try
                {
                    var allRatingComments = await mediator.Send(new GetAllRatingCommentsQuery());
                    return Ok(allRatingComments);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error getting all rating comments: , {ex.Message}, inner: , {ex.InnerException}");
                }
            }

            if (!validFilters.Contains(filter))
                return BadRequest($"Invalid filter: '{filter}'");

            if (string.IsNullOrWhiteSpace(value))
                return BadRequest("Value cannot be empty");

            var filteredComments = await mediator.Send(new GetFilteredRatingCommentsQuery(filter, value));

            return (filteredComments == null || !filteredComments.Any()) ? NotFound() : Ok(filteredComments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RatingComment>> GetRatingCommentById(int id)
        {
            var ratingComment = await mediator.Send(new GetRatingCommentByIdQuery(id));
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
                var newRatingCommentId = await mediator.Send(new AddRatingCommentCommand(newRatingsComments));
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

            var ratingCommentToUpdate = await mediator.Send(new GetRatingCommentByIdQuery(id));

            if (ratingCommentToUpdate== null)
                return NotFound();

            if (userId != ratingCommentToUpdate.UserId)
                return Forbid();

            try
            {
                var updated = await mediator.Send(new UpdateRatingCommentCommand(ratingsComments));
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

            var ratingCommentToDelete = await mediator.Send(new GetRatingCommentByIdQuery(id));

            if(ratingCommentToDelete == null)
                return NotFound();
            
            if (userId != ratingCommentToDelete.UserId)
                return Forbid();
            
            try
            {
                var deleted = await mediator.Send(new DeleteRatingCommentCommand(id));
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting rating comment: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }
    }
}
