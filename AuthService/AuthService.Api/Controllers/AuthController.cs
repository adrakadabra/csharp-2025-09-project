using AuthService.Api.Models.Requests;
using AuthService.Api.Models.Responses;
using AuthService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IKeycloakService _keycloakService;
    
    public AuthController(IKeycloakService keycloakService)
    {
        _keycloakService = keycloakService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Username and password are required");
        }

        var result = await _keycloakService.LoginAsync(request);

        if (result == null)
        {
            return Unauthorized("Invalid username or password");
        }
        
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _keycloakService.RegisterUserAsync(request);

        if (result)
        {
            return Ok(new { message = "Registration successful" });
        }
        
        return BadRequest(new { message = "Registration failed" });
    }
}