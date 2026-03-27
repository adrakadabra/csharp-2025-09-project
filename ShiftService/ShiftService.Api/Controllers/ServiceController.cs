using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ShiftService.Api.Attributes;
using ShiftService.Api.DTO;
namespace ShiftService.Api.Controllers
{
    /// <summary>
    /// Контроллер для аутентификации служебного пользователя
    /// </summary>
    [ApiController]
    [Route("api/service")]
    public class ServiceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ILogger<ServiceController> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// Вход служебного пользователя
        /// Используется при старте React приложения
        /// </summary>
        [HttpPost("service-login")]
        [AllowAnonymous]
        [ServiceApiKey] //защита, проверка X-Service-App-Key
        public async Task<IActionResult> ServiceLogin()
        {
            try
            {
                _logger.LogInformation("Попытка входа служебного пользователя");

                var tokenUrl = $"{_configuration["Keycloak:Authority"]}/protocol/openid-connect/token";
                var httpClient = _httpClientFactory.CreateClient();

                var parameters = new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = _configuration["Keycloak:ServiceUser:ClientId"],
                    ["client_secret"] = _configuration["Keycloak:ServiceUser:ClientSecret"]
                };

                // Логируем параметры (без секретов в продакшене!)
                _logger.LogDebug("Token exchange request to {Url} with parameters: {Parameters}",
                    tokenUrl,
                    string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}")));

                var response = await httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(parameters));

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Ошибка входа служебного пользователя: {Error}", error);
                    return Unauthorized(new { error = "Ошибка аутентификации" });
                }

                var content = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<ServiceLoginResponse>(content);

                _logger.LogInformation("Служебный пользователь успешно вошел, токен истекает через {ExpiresIn} сек",
                    tokenResponse.ExpiresIn);

                return Ok(new
                {
                    access_token = tokenResponse.AccessToken,
                    token_type = tokenResponse.TokenType,
                    expires_in = tokenResponse.ExpiresIn
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при входе служебного пользователя");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        /// <summary>
        /// Время сервера utc
        /// </summary>
        [HttpGet("utc-time")]
        public IActionResult GetUtc() => Ok(DateTime.UtcNow);
        
        }
}