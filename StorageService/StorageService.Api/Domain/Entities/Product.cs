namespace StorageService.Api.Domain.Entities;

// Продукт
public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Article { get; set; } = null!;
    public string? Description { get; set; }

    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public Guid ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; } = null!;

    public Guid SectionId { get; set; }
    public Section Section { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;

    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    public void SoftDelete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
