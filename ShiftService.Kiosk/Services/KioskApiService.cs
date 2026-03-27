using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using ShiftService.Kiosk.Models;

namespace ShiftService.Kiosk.Services;

public class KioskApiService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    private string? _accessToken;
    private DateTime _tokenExpiry;

    private const string AppKeyHeader = "X-Service-App-Key";

    public KioskApiService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    /// <summary>
    /// Проверяет наличие и актуальность токена. Если он истекает через 2 минуты или меньше — обновляет.
    /// </summary>
    private async Task EnsureAuthenticatedAsync()
    {
        if (string.IsNullOrEmpty(_accessToken) || DateTime.UtcNow >= _tokenExpiry.AddMinutes(-2))
        {
            await AuthenticateServiceAsync();
        }
    }

    /// <summary>
    /// Выполняет вход сервиса по X-Service-App-Key для получения JWT
    /// </summary>
    private async Task AuthenticateServiceAsync()
    {
        var appKey = _config["ShiftServiceApi:AppKey"];

        if (string.IsNullOrEmpty(appKey))
            throw new InvalidOperationException("Ключ приложения (AppKey) не найден в appsettings.json");

        // Подготавливаем заголовок для авторизации сервиса
        _http.DefaultRequestHeaders.Remove(AppKeyHeader);
        _http.DefaultRequestHeaders.Add(AppKeyHeader, appKey);

        // Запрос на получение токена
        var response = await _http.PostAsync("api/service/service-login", null);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Ошибка авторизации киоска: {response.StatusCode}");

        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();

        if (auth == null) throw new Exception("Сервер вернул пустой токен");

        _accessToken = auth.access_token;
        // Запоминаем время истечения (UTC)
        _tokenExpiry = DateTime.UtcNow.AddSeconds(auth.expires_in);

        // Устанавливаем Bearer токен для всех последующих запросов через этот HttpClient
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
    }

    // --- МЕТОДЫ API ---

    public async Task<EmployeeResponse?> GetEmployeeByQrCode(string qrCode)
    {
        await EnsureAuthenticatedAsync();
        return await _http.GetFromJsonAsync<EmployeeResponse>($"api/employees/by-qrcode/{qrCode}");
    }

    public async Task<ShiftResponse?> StartShift(Guid employeeId)
    {
        await EnsureAuthenticatedAsync();
        var response = await _http.PostAsJsonAsync("api/shifts/start", new { employeeId });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ShiftResponse>();
    }

    public async Task<ShiftResponse?> EndShift(Guid employeeId)
    {
        await EnsureAuthenticatedAsync();
        var response = await _http.PostAsJsonAsync("api/shifts/end", new { employeeId });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ShiftResponse>();
    }

    public async Task<int> GetActiveNowCount()
    {
        await EnsureAuthenticatedAsync();
        return await _http.GetFromJsonAsync<int>("api/shifts/active/now");
    }

    public async Task<ShiftStatisticsResponse?> GetTodayStatistics(string timeZoneId = "Russian Standard Time")
    {
        await EnsureAuthenticatedAsync();
        return await _http.GetFromJsonAsync<ShiftStatisticsResponse>($"api/shifts/statistics/today?timeZoneId={timeZoneId}");
    }

    public async Task<DateTime> GetServerUtcTime()
    {
        // Для этого метода токен может не требоваться, но вызовем на всякий случай
        await EnsureAuthenticatedAsync();
        return await _http.GetFromJsonAsync<DateTime>("api/service/utc-time");
    }

    public async Task<ShiftResponse?> GetCurrentShift(Guid employeeId)
    {
        try
        {
            return await _http.GetFromJsonAsync<ShiftResponse>($"api/shifts/current/{employeeId}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // 404 означает, что активной смены нет
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении смены: {ex.Message}");
            return null;
        }
    }


}