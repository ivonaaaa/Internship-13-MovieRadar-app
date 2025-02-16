using Microsoft.AspNetCore.Mvc;
using MovieRadar.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MediatR;

namespace MovieRadar.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingReactionController : ControllerBase
    {
        private readonly IMediator mediator;
        
        public RatingReactionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatingReaction>>> GetAllReactions([FromQuery] string? filter, [FromQuery] string? value)
        {
            try
            {
                var allReactions = await mediator.Send(new GetAllRatingReactionsQuery());
                return Ok(allReactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting all reactions: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }
         
        [HttpGet("{id}")]
        public async Task<ActionResult<RatingReaction>> GetById(int id)
        {
            try
            {
                var reaction = await mediator.Send(new GetRatingReactionByIdQuery(id));
                if (reaction == null)
                    return NotFound();

                return Ok(reaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting reaction by id: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddReaction([FromBody] RatingReaction newReaction)
        {
            try
            {
                var newMovieId = await mediator.Send(new AddRatingReactionCommand(newReaction));
                return CreatedAtAction(nameof(GetById), new { id = newMovieId }, newReaction);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding new reaction: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReaction([FromBody] RatingReaction updatedReaction, int id)
        {
            if(id != updatedReaction.Id)
                return BadRequest("Not matching id");

            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
                return Unauthorized();

            var reactionToUpdate = await mediator.Send(new GetRatingReactionByIdQuery(id));

            if (reactionToUpdate == null)
                return NotFound();

            if (userId != reactionToUpdate.UserId)
                return Forbid();

            try
            {
                var updated = await mediator.Send(new UpdateRatingReactionCommand(updatedReaction));
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating new reaction: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReaction(int id)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
                return Unauthorized();

            var reactionToDelete = await mediator.Send(new GetRatingReactionByIdQuery(id));

            if(reactionToDelete == null)
                return NotFound();

            if (userId != reactionToDelete.UserId)
                return Forbid();

            try
            {
                var deleted = await mediator.Send(new DeleteRatingReactionCommand(id));
                return deleted ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting reaction: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByRatingId(int id)
        {
            try
            {
                var reactionsByRatingId = await mediator.Send(new GetFilteredRatingReactionsQuery(id));
                return (reactionsByRatingId == null || !reactionsByRatingId.Any()) ? NotFound() : Ok(reactionsByRatingId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting reaction: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }
    }
}
