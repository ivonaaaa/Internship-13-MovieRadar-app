using Microsoft.AspNetCore.Mvc;
using MovieRadar.Domain.Entities;
using MovieRadar.Application.Services;

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

        [HttpPost]
        public async Task<ActionResult> AddUser(User newUser)
        {
            var id = await userService.Add(newUser);
            return CreatedAtAction(nameof(GetUserById), new { id }, newUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(User updatedUser, int id)
        {
            if (id != updatedUser.Id)
                return BadRequest();

            var updated = await userService.Update(updatedUser);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await userService.DeleteById(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}