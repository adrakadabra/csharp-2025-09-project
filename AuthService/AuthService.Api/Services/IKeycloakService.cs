using AuthService.Api.Models.Requests;
using AuthService.Api.Models.Responses;

namespace AuthService.Api.Services;

public interface IKeycloakService
{
    Task<bool> IsKeycloakHealthyAsync();
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task<bool> RegisterUserAsync(RegisterRequest request);
}