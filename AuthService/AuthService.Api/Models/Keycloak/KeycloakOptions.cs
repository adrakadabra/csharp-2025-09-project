namespace AuthService.Api.Models.Keycloak;

public class KeycloakOptions
{
    public string Authority { get; set; } = string.Empty;
    public string TokenEndpoint { get; set; } = string.Empty;
    public string AdminBaseUrl { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string AdminUsername { get; set; } = string.Empty; 
    public string AdminPassword { get; set; } = string.Empty; 
}