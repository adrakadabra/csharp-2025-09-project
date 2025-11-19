using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Infrastructure.Repositories;

public interface IProductRepository
{
    Task<Product> AddAsync(Product product);
    Task<Product?> GetByIdAsync(Guid id);
    Task<(Product[] items, long total)> GetPagedAsync(int page, int pageSize);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}
