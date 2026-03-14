using OrdersService.Api.Infrastructure.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OrdersService.Api.Infrastructure.Clients;

public class WarehouseClient : IWarehouseClient
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public WarehouseClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProductInfo>> GetProductsByIdsAsync(
        IEnumerable<Guid> productIds,
        string jwtToken,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/Products/by-ids")
        {
            Content = JsonContent.Create(productIds.ToList())
        };

        AddBearer(request, jwtToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<ProductInfo>>(JsonOptions, cancellationToken);
        return result ?? [];
    }

    public async Task<ReservedOrderInfo> ReserveProductsAsync(
        ReserveProductsRequest requestModel,
        string jwtToken,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/Reservations/reserve")
        {
            Content = JsonContent.Create(requestModel)
        };

        AddBearer(request, jwtToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ReservedOrderInfo>(JsonOptions, cancellationToken);
        if (result is null)
            throw new InvalidOperationException("Сервис склада вернул пустой ответ при резервировании.");

        return result;
    }

    public async Task CancelReservationAsync(
        Guid orderNumber,
        string jwtToken,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"/Reservations/cancel/{orderNumber}");
        AddBearer(request, jwtToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<ReservedOrderInfo?> GetReservationAsync(
        Guid orderNumber,
        string jwtToken,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/Reservations/{orderNumber}");
        AddBearer(request, jwtToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
         
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ReservedOrderInfo>(JsonOptions, cancellationToken);
    }

    private static void AddBearer(HttpRequestMessage request, string jwtToken)
    {
        if (!string.IsNullOrWhiteSpace(jwtToken))
        {
            var token = jwtToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? jwtToken["Bearer ".Length..]
                : jwtToken;

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public Task CompleteReservationAsync(Guid orderNumber, string jwtToken, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}