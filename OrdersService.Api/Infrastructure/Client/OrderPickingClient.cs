using System.Net.Http.Headers;

namespace OrdersService.Api.Infrastructure.Clients;

public class OrderPickingClient : IOrderPickingClient
{
    private readonly HttpClient _httpClient;

    public OrderPickingClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AssemblyOrderResponse> CreateOrderAsync(
        CreateAssemblyOrderRequest requestModel,
        string jwtToken,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/Order/CreateOrder")
        {
            Content = JsonContent.Create(requestModel)
        };

        if (!string.IsNullOrWhiteSpace(jwtToken))
        {
            var token = jwtToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? jwtToken["Bearer ".Length..]
                : jwtToken;

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AssemblyOrderResponse>(cancellationToken: cancellationToken);

        if (result is null)
            throw new InvalidOperationException("OrderPickingService вернул пустой ответ.");

        return result;
    }
}