using Microsoft.AspNetCore.Mvc;
using MovieRadar.Application.Interfaces;
using MovieRadar.Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;
using MediatR;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private IMediator mediator;
    private readonly ITokenService tokenService;

    public AuthController(IMediator mediator, ITokenService tokenService)
    {
        this.mediator = mediator;
        this.tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await mediator.Send(new GetUserByEmailQuery(request.Email));

        if (user == null || user.Password != request.Password)
        {
            return Unauthorized(new { message = "Invalid email or password!" });
        }

        var token = tokenService.GenerateToken(user.Id, user.Email, user.IsAdmin);
        return Ok(new { token, user.IsAdmin });
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] User newUser)
    {
        try
        {
            var id = await mediator.Send(new AddUserCommand(newUser));
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error adding new user: , {ex.Message}, inner: , {ex.InnerException}");
        }
    }
}