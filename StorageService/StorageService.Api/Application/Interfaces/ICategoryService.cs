using StorageService.Api.Application.DTOs;

namespace StorageService.Api.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetOrCreateAsync(string name);
        Task HandleUnusedAsync(Guid categoryId);
        Task<CategoryDto?> GetAsync(Guid id);
        Task<List<CategoryDto>> GetAllAsync();
    }
}
