namespace OrdersService.Api.Infrastructure.Warehouse
{
    public interface IWarehouseClient
    {
        Task<List<ProductInfo>> GetProductsByIdsAsync(IEnumerable<int> productIds, string jwtToken);
    }
}