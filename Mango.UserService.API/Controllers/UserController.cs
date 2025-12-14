using Mango.UserService.Application.DTOs;
using Mango.UserService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
  [Route("api/v1/users")]
  [ApiController]
    public class UserController : ControllerBase
    {
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
      _userService = userService;
    }

    /// <summary>
    /// Register a new user (Farmer, Reseller, or Wholesaler)
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var result = await _userService.RegisterAsync(request, cancellationToken);
      return CreatedAtAction(nameof(Register), new { id = result.UserId }, result);
    }


    //[HttpPost("login")]
    //public IActionResult Login([FromBody] LoginRequest request)
    //{
    //  return Ok(new { token = "jwt-token-placeholder" });
    //}

    [HttpGet("me")]
    public IActionResult Me()
    {
      return Ok(new { id = "123", fullName = "Test User", role = "Farmer" });
    }
  }
}
