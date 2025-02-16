using Microsoft.AspNetCore.Mvc;
using MovieRadar.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using MediatR;

namespace MovieRadar.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var allUsers = await mediator.Send(new GetAllUsersQuery());
            return Ok(allUsers);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await mediator.Send(new GetUserByIdQuery(id));
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] User updatedUser, int id)
        {
            if (id != updatedUser.Id)
                return BadRequest("Not matching user ID");

            try
            {
                var updated = await mediator.Send(new UpdateUserCommand(updatedUser));
                return updated ? NoContent() : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating user: , {ex.Message}, inner: , {ex.InnerException}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await mediator.Send(new DeleteUserCommand(id));
            return deleted ? NoContent() : NotFound();
        }
    }
}