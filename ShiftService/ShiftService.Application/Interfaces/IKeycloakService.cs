using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShiftService.Application.Interfaces
{
    /// <summary>
    /// МИНИМАЛЬНЫЙ интерфейс для работы с Keycloak
    /// Только то, что нужно для QR-логина
    /// </summary>
    public interface IKeycloakService
    {
        /// <summary>
        /// Получить временный токен от Keycloak (client_credentials)
        /// </summary>
        Task<string> GetTemporaryTokenAsync();

        /// <summary>
        /// Получить постоянный токен для пользователя через token exchange
        /// </summary>
        Task<KeycloakTokenResponse> ExchangeTokenAsync(string temporaryToken, string keycloakUserId);
    }

    /// <summary>
    /// Ответ с токеном от Keycloak
    /// </summary>
    public class KeycloakTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }      // Постоянный токен доступа
        
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }     // Токен обновления
        
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }           // Время жизни (секунды)

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }        // Bearer
    }
}