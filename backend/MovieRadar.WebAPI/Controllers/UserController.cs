using Microsoft.AspNetCore.Mvc;
using MovieRadar.Domain.Entities;
using MovieRadar.Application.Services;
using MovieRadar.Application.Queries;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var allUsers = await mediator.Send(new GetAll());
            return Ok(allUsers);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUserById(int id)
        //{
        //    var user = await userService.GetById(id);
        //    if (user == null)
        //        return NotFound();

        //    return Ok(user);
        //}

        //[HttpPost]
        //public async Task<ActionResult> AddUser([FromBody] User newUser)
        //{
        //    var id = await userService.Add(newUser);
        //    return CreatedAtAction(nameof(GetUserById), new { id }, newUser);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateUser([FromBody] User updatedUser, int id)
        //{
        //    if (id != updatedUser.Id)
        //        return BadRequest();

        //    var updated = await userService.Update(updatedUser);
        //    return updated ? NoContent() : NotFound();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var deleted = await userService.DeleteById(id);
        //    return deleted ? NoContent() : NotFound();
        //}
    }
}