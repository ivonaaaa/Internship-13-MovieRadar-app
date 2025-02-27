﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRadar.Domain.Entities;
using System.Security.Claims;

namespace MovieRadar.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IMediator mediator;
        public RatingController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> GetAllRatings([FromQuery] string? filter, [FromQuery] string? value)
        {
            var validFilters = new HashSet<string> { "movie_id" };

            if (string.IsNullOrEmpty(filter) && string.IsNullOrEmpty(value))
            {
                var allRatings = await mediator.Send(new GetAllRatingsQuery());
                return (allRatings == null || !allRatings.Any()) ? NotFound() : Ok(allRatings);
            }

            if (!validFilters.Contains(filter))
                return BadRequest($"Invalid filter: '{filter}'");

            if (string.IsNullOrWhiteSpace(value))
                return BadRequest("Value cannot be empty");

            var filteredRatings = await mediator.Send(new GetFilteredRatingsQuery(filter, value));

            return (filteredRatings == null || !filteredRatings.Any()) ? NotFound() : Ok(filteredRatings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetRatingById(int id)
        {
            var rating = await mediator.Send(new GetRatingByIdQuery(id));
            return rating == null ? NotFound() : Ok(rating);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddRating([FromBody] Rating newRating)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
                return Unauthorized();

            if (userId != newRating.UserId)
                return Forbid();

            try
            {
                var id = await mediator.Send(new AddRatingCommand(newRating));
                return CreatedAtAction(nameof(GetRatingById), new { id }, newRating);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRating([FromBody] Rating updatedRating, int id)
        {
            if(id != updatedRating.Id)
                return BadRequest("Not matching rating ID");

            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
                return Unauthorized();

            var ratingToUpdate = await mediator.Send(new GetRatingByIdQuery(id));

            if (ratingToUpdate == null)
                return NotFound();

            if (userId != ratingToUpdate.UserId)
                return Forbid();

            try
            {
                var updated = await mediator.Send(new UpdateRatingCommand(updatedRating));
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
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
                return Unauthorized();

            var ratingToDelete = await mediator.Send(new GetRatingByIdQuery(id));

            if (ratingToDelete == null)
                return NotFound();

            if (userId != ratingToDelete.UserId)
                return Forbid();

            var deleted = await mediator.Send(new DeleteRatingCommand(id));
            return deleted ? NoContent() : NotFound();
        }
    }
}
