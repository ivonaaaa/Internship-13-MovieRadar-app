using Microsoft.AspNetCore.Mvc;
using MovieRadar.Application.Interfaces;
using MovieRadar.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MovieRadar.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingReactionsController : ControllerBase
    {
        private readonly IRatingReactionsService ratingReactionsService;
        
        public RatingReactionsController(IRatingReactionsService ratingReactionsService)
        {
            this.ratingReactionsService = ratingReactionsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatingsReactions>>> GetAllReactions()
        {
            try
            {
                var allReactions = await ratingReactionsService.GetAll();
                return Ok(allReactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting all reactions: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }
         
        [HttpGet("{id}")]
        public async Task<ActionResult<RatingsReactions>> GetById(int id)
        {
            try
            {
                var reaction = await ratingReactionsService.GetById(id);
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
        public async Task<ActionResult> AddReaction([FromBody] RatingsReactions newReaction)
        {
            try
            {
                var newMovieId = await ratingReactionsService.Add(newReaction);
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
        public async Task<IActionResult> UpdateReaction([FromBody] RatingsReactions updatedReaction, int id)
        {
            if(id != updatedReaction.Id)
                return BadRequest("Not matching id");

            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
                return Unauthorized();

            if (userId != updatedReaction.UserId)
                return Forbid();

            try
            {
                var updated = await ratingReactionsService.Update(updatedReaction);
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

            if (userId != id)
                return Forbid();

            try
            {
                var deleted = await ratingReactionsService.DeleteById(id);
                return deleted ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting reaction: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }

        [HttpGet("ratingId/{id}")]
        public async Task<IActionResult> GetAllByRatingId(int id)
        {
            try
            {
                Console.WriteLine("aaaaaa" + id);
                var reactionsByRatingId = await ratingReactionsService.GetAllReactionsByRatingId(id);
                
                if(reactionsByRatingId == null || !reactionsByRatingId.Any())
                    return NotFound();
                
                return Ok(reactionsByRatingId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting reaction: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }
    }
}
