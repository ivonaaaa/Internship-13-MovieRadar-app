using Microsoft.AspNetCore.Mvc;
using MovieRadar.Domain.Entities;
using MovieRadar.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace MovieRadar.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var allUsers = await userService.GetAll();
            return Ok(allUsers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await userService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] User updatedUser, int id)
        {
            if (id != updatedUser.Id)
                return BadRequest();

            var updated = await userService.Update(updatedUser);
            return updated ? NoContent() : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await userService.DeleteById(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}