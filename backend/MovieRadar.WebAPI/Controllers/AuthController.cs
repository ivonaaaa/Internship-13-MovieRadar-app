using Microsoft.AspNetCore.Mvc;
using MovieRadar.Application.Services;
using MovieRadar.Domain.Entities;
using MovieRadar.Application.Helpers;
using Microsoft.AspNetCore.Identity.Data;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService userService;
    private readonly TokenService _tokenService;

    public AuthController(IUserService userService, TokenService tokenService)
    {
        this.userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await userService.GetByEmail(request.Email);

        if (user == null || user.Password != request.Password)
        {
            return Unauthorized(new { message = "Invalid email or password!" });
        }

        var token = _tokenService.GenerateToken(user.Id, user.Email);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] User newUser)
    {
        if (!UserHelper.isUserValid(newUser))
        {
            return BadRequest("Invalid register request!");
        }

        var user = await userService.GetByEmail(newUser.Email);

        if (user != null)
        {
            return BadRequest(new { message = "Email is already taken!" });
        }

        var id = await userService.Add(newUser);
        return Ok();
    }
}