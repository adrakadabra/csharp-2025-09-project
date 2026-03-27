using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShiftService.Application.Interfaces;

namespace ShiftService.Infrastructure.Services
{
    /// <summary>
    /// МИНИМАЛЬНАЯ реализация для работы с Keycloak
    /// Только то, что нужно для QR-логина
    /// </summary>
    public class KeycloakService : IKeycloakService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<KeycloakService> _logger;

        public KeycloakService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<KeycloakService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Получить временный токен от Keycloak (client_credentials)
        /// </summary>
        public async Task<string> GetTemporaryTokenAsync()
        {
            try
            {
                // URL для получения токена
                var tokenUrl = $"{_configuration["Keycloak:Authority"]}/protocol/openid-connect/token";

                _logger.LogInformation("Запрос временного токена у Keycloak");

                // Параметры запроса
                var parameters = new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = _configuration["Keycloak:ClientId"],
                    ["client_secret"] = _configuration["Keycloak:ClientSecret"]
                };

                var requestBody = new FormUrlEncodedContent(parameters);

                // Отправляем запрос
                var response = await _httpClient.PostAsync(tokenUrl, requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Ошибка получения токена: {StatusCode} - {Error}",
                        response.StatusCode, error);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<KeycloakTokenResponse>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                _logger.LogInformation("Временный токен получен");
                return tokenResponse?.AccessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении временного токена");
                return null;
            }
        }

        /// <summary>
        /// Обменять временный токен на постоянный токен пользователя
        /// </summary>
        //public async Task<KeycloakTokenResponse> ExchangeTokenAsync(string temporaryToken, string keycloakUserId)
        //{
        //    try
        //    {
        //        // URL для обмена токена
        //        var tokenUrl = $"{_configuration["Keycloak:Authority"]}/protocol/openid-connect/token";

        //        _logger.LogInformation("Обмен временного токена на постоянный для пользователя {UserId}", keycloakUserId);

        //        // Параметры для token exchange
        //        var parameters = new Dictionary<string, string>
        //        {
        //            ["grant_type"] = "urn:ietf:params:oauth:grant-type:token-exchange",
        //            ["client_id"] = _configuration["Keycloak:ClientId"],                 //id Keycloak user
        //            ["client_secret"] = _configuration["Keycloak:ClientSecret"],         // 
        //            ["subject_token"] = temporaryToken,
        //            ["requested_subject"] = keycloakUserId,  // ID пользователя, для которого нужен токен
        //            ["requested_token_type"] = "urn:ietf:params:oauth:token-type:access_token",
        //            ["scope"] = "openid offline_access"
        //        };

        //        var requestBody = new FormUrlEncodedContent(parameters);

        //        // Логируем параметры (без секретов в продакшене!)
        //        _logger.LogDebug("Token exchange request to {Url} with parameters: {Parameters}",
        //            tokenUrl,
        //            string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}")));

        //        // Отправляем запрос
        //        var response = await _httpClient.PostAsync(tokenUrl, requestBody);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            var error = await response.Content.ReadAsStringAsync();
        //            _logger.LogError("Ошибка обмена токена: {StatusCode} - {Error}",
        //                response.StatusCode, error);
        //            return null;
        //        }

        //        var content = await response.Content.ReadAsStringAsync();
        //        var tokenResponse = JsonSerializer.Deserialize<KeycloakTokenResponse>(content,
        //            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //        _logger.LogInformation("Постоянный токен получен для пользователя {UserId}", keycloakUserId);
        //        return tokenResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Ошибка при обмене токена");
        //        return null;
        //    }
        //}
        /// <summary>
        /// Получить токен пользователя напрямую с offline_access (рекомендуемый способ)
        /// </summary>
        public async Task<KeycloakTokenResponse> ExchangeTokenAsync(string temporaryToken, string keycloakUserId)
        {
            try
            {
                var tokenUrl = $"{_configuration["Keycloak:Authority"]}/protocol/openid-connect/token";

                _logger.LogInformation("Получение токена пользователя {UserId}", keycloakUserId);

                // Получаем токен клиента для аутентификации
                var clientToken = await GetTemporaryTokenAsync();
                if (string.IsNullOrEmpty(clientToken))
                {
                    _logger.LogError("Не удалось получить клиентский токен");
                    return null;
                }

                // Используем token exchange с impersonation
                var parameters = new Dictionary<string, string>
                {
                    ["grant_type"] = "urn:ietf:params:oauth:grant-type:token-exchange",
                    ["client_id"] = _configuration["Keycloak:ClientId"],
                    ["client_secret"] = _configuration["Keycloak:ClientSecret"],
                    ["subject_token"] = clientToken,
                    //  ["subject_issuer"] = "client",  // Важно: указываем, что subject - это клиент
                    // ["subject_token_type"] = "urn:ietf:params:oauth:token-type:access_token",
                    ["requested_subject"] = keycloakUserId,
                    ["requested_token_type"] = "urn:ietf:params:oauth:token-type:access_token",
                    ["scope"] = "openid offline_access"
                };


                // Логируем параметры (без секретов в продакшене!)
                        _logger.LogDebug("Token exchange request to {Url} with parameters: {Parameters}",
                            tokenUrl,
                            string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}")));

                var requestBody = new FormUrlEncodedContent(parameters);
                var response = await _httpClient.PostAsync(tokenUrl, requestBody);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка получения токена пользователя: {StatusCode} - {Error}",
                        response.StatusCode, content);
                }

                var tokenResponse = JsonSerializer.Deserialize<KeycloakTokenResponse>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (tokenResponse != null)
                {
                    if (!string.IsNullOrEmpty(tokenResponse.RefreshToken))
                    {
                        _logger.LogInformation("Токен пользователя получен с refresh_token. Действует {ExpiresIn} сек",
                            tokenResponse.ExpiresIn);
                    }
                    else
                    {
                        _logger.LogWarning("Токен получен, но без refresh_token");
                    }
                }

                return tokenResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении токена пользователя");
                return null;
            }
        }
    }
}