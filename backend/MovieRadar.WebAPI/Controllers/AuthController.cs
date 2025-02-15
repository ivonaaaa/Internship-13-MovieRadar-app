using Microsoft.AspNetCore.Mvc;
using MovieRadar.Application.Services;
using MovieRadar.Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService userService;
    private readonly ITokenService tokenService;

    public AuthController(IUserService userService, ITokenService tokenService)
    {
        this.userService = userService;
        this.tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await userService.GetByEmail(request.Email);

        if (user == null || user.Password != request.Password)
        {
            return Unauthorized(new { message = "Invalid email or password!" });
        }

        var token = tokenService.GenerateToken(user.Id, user.Email, user.IsAdmin);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] User newUser)
    {
        try
        {
            var id = await userService.Add(newUser);
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