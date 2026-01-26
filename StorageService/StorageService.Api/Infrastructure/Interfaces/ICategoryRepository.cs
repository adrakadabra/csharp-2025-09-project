using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Infrastructure.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category?> GetByNameAsync(string name);
        Task<List<Category>> GetAllAsync();
        Task AddAsync(Category category);
        Task<bool> HasProductsAsync(Guid categoryId);
        Task DeleteAsync(Guid categoryId);
    }
}
