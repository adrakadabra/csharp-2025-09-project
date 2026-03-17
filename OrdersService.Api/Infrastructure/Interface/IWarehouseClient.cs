using OrdersService.Api.Infrastructure.Clients;

namespace OrdersService.Api.Infrastructure.Interfaces;

public interface IWarehouseClient
{
    Task<List<ProductInfo>> GetProductsByIdsAsync(
        IEnumerable<Guid> productIds,
        string jwtToken,
        CancellationToken cancellationToken = default);
}