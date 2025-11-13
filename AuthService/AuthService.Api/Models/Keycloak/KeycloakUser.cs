using System.Text.Json.Serialization;

namespace AuthService.Api.Models.Keycloak;

public class KeycloakUser
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = true;

    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; } = true;

    [JsonPropertyName("credentials")]
    public List<KeycloakCredential> Credentials { get; set; } = new();
}