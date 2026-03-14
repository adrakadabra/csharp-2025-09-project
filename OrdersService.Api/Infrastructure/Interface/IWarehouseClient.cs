using OrdersService.Api.Infrastructure.Clients;

namespace OrdersService.Api.Infrastructure.Interfaces;

public interface IWarehouseClient
{
    Task<List<ProductInfo>> GetProductsByIdsAsync(
        IEnumerable<Guid> productIds,
        string jwtToken,
        CancellationToken cancellationToken = default);

    Task<ReservedOrderInfo> ReserveProductsAsync(
        ReserveProductsRequest request,
        string jwtToken,
        CancellationToken cancellationToken = default);

    Task CancelReservationAsync(
        Guid orderNumber,
        string jwtToken,
        CancellationToken cancellationToken = default);

    Task CompleteReservationAsync(
        Guid orderNumber,
        string jwtToken,
        CancellationToken cancellationToken = default);

    Task<ReservedOrderInfo?> GetReservationAsync(
        Guid orderNumber,
        string jwtToken,
        CancellationToken cancellationToken = default);
}