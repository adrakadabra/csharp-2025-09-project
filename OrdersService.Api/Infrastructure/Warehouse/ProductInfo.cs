namespace OrdersService.Api.Infrastructure.Warehouse
{
    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}