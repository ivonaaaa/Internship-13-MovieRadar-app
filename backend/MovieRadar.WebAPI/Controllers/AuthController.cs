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
        var userValidity = UserHelper.isUserValid(newUser);

        if (!userValidity.Item1)
        {
            return BadRequest(new { message = userValidity.Item2 });
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