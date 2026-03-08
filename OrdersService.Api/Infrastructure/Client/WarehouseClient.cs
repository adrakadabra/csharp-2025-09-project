using System.Net.Http.Headers;
using System.Net.Http.Json;
using OrdersService.Api.Infrastructure.Interfaces;

namespace OrdersService.Api.Infrastructure.Clients;

public class WarehouseClient : IWarehouseClient
{
    private readonly HttpClient _httpClient;

    public WarehouseClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProductInfo>> GetProductsByIdsAsync(
        IEnumerable<Guid> productIds,
        string jwtToken,
        CancellationToken cancellationToken = default)
    {
        var ids = productIds.Distinct().ToList();

        if (ids.Count == 0)
            return [];

        using var request = new HttpRequestMessage(HttpMethod.Post, "/products/by-ids");

        if (!string.IsNullOrWhiteSpace(jwtToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        }

        request.Content = JsonContent.Create(ids);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<List<ProductInfo>>(cancellationToken: cancellationToken);
        return products ?? [];
    }
}