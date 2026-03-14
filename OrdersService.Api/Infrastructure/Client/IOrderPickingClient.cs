namespace OrdersService.Api.Infrastructure.Clients;

public interface IOrderPickingClient
{
    Task<AssemblyOrderResponse> CreateOrderAsync(
        CreateAssemblyOrderRequest request,
        string jwtToken,
        CancellationToken cancellationToken = default);
}