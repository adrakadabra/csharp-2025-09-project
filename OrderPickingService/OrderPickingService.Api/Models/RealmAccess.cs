using System.Text.Json.Serialization;

namespace OrderPickingService.Api.Models;

public class RealmAccess
{
    [JsonPropertyName("roles")]
    public List<string> Roles { get; set; } = new();
}