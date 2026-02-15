using StorageService.Api.Application.DTOs;
using StorageService.Api.Application.Interfaces;
using StorageService.Api.Application.Mappers;
using StorageService.Api.Domain.Entities;
using StorageService.Api.Infrastructure.Interfaces;

namespace StorageService.Api.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<CategoryDto> GetOrCreateAsync(string name)
        {
            var category = await _repo.GetByNameAsync(name);

            if (category != null)
                return category.ToDto();

            category = new Category
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            await _repo.AddAsync(category);
            return category.ToDto();
        }

        public async Task HandleUnusedAsync(Guid categoryId)
        {
            if (await _repo.HasProductsAsync(categoryId))
                return;

            await _repo.DeleteAsync(categoryId);
        }

        public async Task<CategoryDto?> GetAsync(Guid id)
        {
            var category = await _repo.GetByIdAsync(id);

            return category?.ToDto();
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();

            return categories.Select(c => c.ToDto()).ToList();
        }
    }
}
