using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Infrastructure.Data;

/// <summary>
/// Демо-данные для тестирования
/// </summary>
public static class FakeProductsData
{
    /// <summary>
    /// Продукты
    /// </summary>
    public static List<Product> Products { get; } = new List<Product>
    {
        new Product
        {
            Id = new Guid(),
            CreatedAt = DateTime.UtcNow,
            Description = "test",
            IsDeleted = false,
            Name = "TestName",
            Quantity = 0,
            Price = 0
        }
    };
}