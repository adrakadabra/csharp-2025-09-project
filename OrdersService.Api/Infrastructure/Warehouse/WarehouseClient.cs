
namespace OrdersService.Api.Infrastructure.Warehouse
{
    public class WarehouseClient : IWarehouseClient
    {
        private readonly HttpClient _httpClient;
        public WarehouseClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductInfo>> GetProductsByIdsAsync(IEnumerable<int> productIds, string jwtToken)
        {
            var idsList = productIds.ToList();
            if (!idsList.Any())
                return new List<ProductInfo>();

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "/api/orders/by-ids");

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            request.Content = JsonContent.Create(idsList);

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadFromJsonAsync<List<ProductInfo>>();
            return products ?? new List<ProductInfo>();
        }
    }
}