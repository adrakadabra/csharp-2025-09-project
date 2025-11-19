using System.Globalization;
using System.Text;
using AuthService.Api.Models;
using AuthService.Api.Models.Requests;
using Microsoft.Extensions.Options;
using System.Text.Json;
using AuthService.Api.Models.Keycloak;
using AuthService.Api.Models.Responses;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace AuthService.Api.Services;

public class KeycloakService : IKeycloakService
{
    private readonly HttpClient _httpClient;
    private readonly KeycloakOptions _options;

    public KeycloakService(
        IHttpClientFactory httpClientFactory,
        IOptions<KeycloakOptions> options)
    {
        _httpClient = httpClientFactory.CreateClient("Keycloak");
        _options = options.Value;
    }

    public async Task<bool> IsKeycloakHealthyAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"/realms/csharp-2025-09-project");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        try
        {
            var formData = new List<KeyValuePair<string, string>>
            {
                new("client_id", _options.ClientId),
                new("username", request.Username),
                new("password", request.Password),
                new("grant_type", "password"),
                new("scope", "openid")
            };
            
            var response = await _httpClient.PostAsync(
                _options.TokenEndpoint,
                new FormUrlEncodedContent(formData));
            
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response status: {response.StatusCode}");
            Console.WriteLine($"Response content: {responseContent}");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };
                
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(content);

                if (loginResponse != null)
                {
                    loginResponse.Username = request.Username;
                    
                    var payload = DecodeJwtPayload(loginResponse.AccessToken);
                    loginResponse.UserId = payload.TryGetValue("sub", out var sub) ? sub.ToString() ?? "" : "";
                    
                    if (payload.TryGetValue("realm_access", out var realmAccess) && realmAccess is JsonElement realmElement)
                    {
                        if (realmElement.TryGetProperty("roles", out var rolesElement))
                        {
                            loginResponse.Roles = rolesElement.EnumerateArray()
                                .Select(r => r.GetString() ?? "")
                                .Where(r => !string.IsNullOrEmpty(r))
                                .ToList();
                        }
                    }
                }
                
                return loginResponse; 
            }

            return null; 
        }
        catch (Exception ex)
        {
            //TODO: обработать исключение
            return null; 
        }
    }

    public async Task<bool> RegisterUserAsync(RegisterRequest request)
    {
        try
        {
            Console.WriteLine($"Registration attempt for: {request.Username}");

            var adminToken = await GetAdminAccessTokenAsync();

            if (string.IsNullOrEmpty(adminToken))
            {
                Console.WriteLine("Failed to get admin token");
                return false;
            }

            var user = new KeycloakUser()
            {
                Username = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Enabled = true,
                EmailVerified = true,
                Credentials = new List<KeycloakCredential>
                {
                    new KeycloakCredential
                    {
                        Type = "password",
                        Value = request.Password,
                        Temporary = false
                    }
                }
            };
            
            var createUserUrl = $"{_options.AdminBaseUrl}/users";

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, createUserUrl);
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            
            var response = await _httpClient.SendAsync(requestMessage);
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"User {request.Username} created successfully");
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to create user: {response.StatusCode} - {errorContent}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in RegisterUserAsync: {ex.Message}");
            return false;
        }
    }

    private async Task<string?> GetAdminAccessTokenAsync()
    {
        try
        {
            var formData = new List<KeyValuePair<string, string>>
            {
                new("client_id", _options.ClientId),
                new("username", _options.AdminUsername),
                new("password", _options.AdminPassword),
                new("grant_type", "password")
            };
            
            var response = await _httpClient.PostAsync(
                _options.TokenEndpoint,
                new FormUrlEncodedContent(formData));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<AdminTokenResponse>(content);
                return tokenResponse?.AccessToken;
            }
            
            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private Dictionary<string, object> DecodeJwtPayload(string jwt)
    {
        try
        {
            var parts = jwt.Split('.');
            if (parts.Length != 3) return new Dictionary<string, object>();

            var payload = parts[1];
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            var payloadBytes = Convert.FromBase64String(payload);
            var payloadJson = Encoding.UTF8.GetString(payloadBytes);
        
            return JsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson) ?? new Dictionary<string, object>();
        }
        catch
        {
            return new Dictionary<string, object>();
        }
    }
}