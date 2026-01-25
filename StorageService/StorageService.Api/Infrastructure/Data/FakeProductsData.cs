using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Infrastructure.Data;

/// <summary>
/// Демо-данные для тестирования
/// </summary>
public static class FakeProductsData
{
    private static Guid SectionId = Guid.NewGuid();
    public static List<Section> Sections { get; } = new List<Section>
    {
        new Section
        {
            Id = SectionId,
            Code = "M3",
            Description = "Масла"
        }
    };
    /// <summary>
    /// Продукты
    /// </summary>
    public static List<Product> Products { get; } = new List<Product>
    {
        new Product
        {
            Id = new Guid(),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "Me",
            Description = "Масло 5w-40",
            IsDeleted = false,
            Name = "Машинное масло",
            Quantity = 3,
            Price = 1250,
            Category = new Category
            {
                Id= new Guid(),
                Name = "Автомобильные товары",
                Description = "Автомобильные товары"
            },
            Manufacturer = new Manufacturer
            {
                Id= new Guid(),
                Name = "Shell",
                Country = "Германия"
            },
            SectionId = SectionId,
        }
    };
}