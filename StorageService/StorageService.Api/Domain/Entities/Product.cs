namespace StorageService.Api.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    public void SoftDelete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? description, int quantity, decimal price)
    {
        Name = name;
        Description = description;
        Quantity = quantity;
        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }
}
