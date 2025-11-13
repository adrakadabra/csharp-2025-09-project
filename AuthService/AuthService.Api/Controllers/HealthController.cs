using AuthService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IKeycloakService _keycloakService;

    public HealthController(IKeycloakService keycloakService)
    {
        _keycloakService = keycloakService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var keyCloakStatus = await _keycloakService.IsKeycloakHealthyAsync();
        return Ok(new { 
            status = "Auth Service is running!",
            timestamp = DateTime.UtcNow,
            keycloak = keyCloakStatus ? "connected" : "disconnected"
        });
    }
}