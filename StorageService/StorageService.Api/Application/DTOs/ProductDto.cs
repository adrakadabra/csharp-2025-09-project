namespace StorageService.Api.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public CategoryDto Category { get; set; } = null!;
    public ManufacturerDto Manufacturer { get; set; } = null!;
    public SectionDto Section { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

