using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Infrastructure.Interfaces;

public interface IProductRepository
{
    Task<Product> AddAsync(Product product);
    Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> GetBySectionIdAsync(Guid sectionId);
    Task<Product?> GetByArticleAsync(string article);
    Task<(Product[] items, long total)> GetPagedAsync(int page, int pageSize);
    Task<List<Product>> GetByIdsAsync(List<Guid> ids);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}
