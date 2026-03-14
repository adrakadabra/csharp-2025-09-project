using System.Text.Json;
using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Infrastructure.Data;

public static class JsonSeedDataLoader
{
    public static ProductsSeedJson LoadProductsSeedJson()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "DataSeed", "products.json");

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Не найден файл сидов: {filePath}");

        var json = File.ReadAllText(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var data = JsonSerializer.Deserialize<ProductsSeedJson>(json, options);

        if (data is null)
            throw new InvalidOperationException("Не удалось прочитать products.json");

        return data;
    }

    public static List<Section> BuildSections(ProductsSeedJson data)
    {
        return data.Sections.Select(x => new Section
        {
            Id = x.Id,
            Code = x.Code,
            Description = x.Description
        }).ToList();
    }

    public static List<Category> BuildCategories(ProductsSeedJson data)
    {
        return data.Categories.Select(x => new Category
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            IsDeleted = false
        }).ToList();
    }

    public static List<Manufacturer> BuildManufacturers(ProductsSeedJson data)
    {
        return data.Manufacturers.Select(x => new Manufacturer
        {
            Id = x.Id,
            Name = x.Name,
            Country = x.Country,
            IsDeleted = false
        }).ToList();
    }

    public static List<Product> BuildProducts(ProductsSeedJson data)
    {
        return data.Products.Select(x => new Product
        {
            Id = x.Id,
            Name = x.Name,
            Article = x.Article,
            Description = x.Description,
            Price = x.Price,
            Quantity = x.Quantity,
            CategoryId = x.CategoryId,
            ManufacturerId = x.ManufacturerId,
            SectionId = x.SectionId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "JsonSeed",
            IsDeleted = false
        }).ToList();
    }
}