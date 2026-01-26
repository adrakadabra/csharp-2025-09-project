namespace StorageService.Api.Application.DTOs;

public class UpdateProductDto
{
    public string? Name { get; set; }
    public string? Article { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }

    public string? CategoryName { get; set; }
    public string? ManufacturerName { get; set; }

    public string? SectionCode { get; set; }
}
