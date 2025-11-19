using System.Text.Json.Serialization;

namespace AuthService.Api.Models.Keycloak;

public class KeycloakCredential
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "password";

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("temporary")]
    public bool Temporary { get; set; } = false;
}