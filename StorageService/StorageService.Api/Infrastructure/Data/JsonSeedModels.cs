namespace StorageService.Api.Infrastructure.Data;

public sealed class ProductsSeedJson
{
    public List<SeedSection> Sections { get; set; } = [];
    public List<SeedCategory> Categories { get; set; } = [];
    public List<SeedManufacturer> Manufacturers { get; set; } = [];
    public List<SeedProduct> Products { get; set; } = [];
}

public sealed class SeedSection
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public sealed class SeedCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public sealed class SeedManufacturer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
}

public sealed class SeedProduct
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Article { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public Guid CategoryId { get; set; }
    public Guid ManufacturerId { get; set; }
    public Guid SectionId { get; set; }
}