using Microsoft.AspNetCore.Mvc;
using MovieRadar.Domain.Entities;
using MovieRadar.Application.Services;
using Microsoft.AspNetCore.Authorization;
using MovieRadar.Application.Interfaces;
using System.Security.Claims;
using MovieRadar.Application.Helpers;

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
                return BadRequest("Not matching user ID");

            try
            {
                var updated = await userService.Update(updatedUser);
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
            var deleted = await userService.DeleteById(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}