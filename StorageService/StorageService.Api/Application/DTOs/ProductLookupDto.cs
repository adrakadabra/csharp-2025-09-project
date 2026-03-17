namespace StorageService.Api.Application.DTOs;

public class ProductLookupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
}