using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Application.DTOs;

public class ProductLookupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Article { get; set; } = null!;
    public int Quantity { get; set; }
    public int AvailableQuantity { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = null!;
}