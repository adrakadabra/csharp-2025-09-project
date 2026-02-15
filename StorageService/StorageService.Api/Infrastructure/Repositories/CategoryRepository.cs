using Microsoft.EntityFrameworkCore;
using StorageService.Api.Domain.Entities;
using StorageService.Api.Infrastructure.Data;
using StorageService.Api.Infrastructure.Interfaces;

namespace StorageService.Api.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<Category?> GetByIdAsync(Guid id) =>
            _db.Categories.FirstOrDefaultAsync(x => x.Id == id);

        public Task<Category?> GetByNameAsync(string name) =>
            _db.Categories.FirstOrDefaultAsync(x => x.Name == name);

        public async Task AddAsync(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
        }

        public Task<bool> HasProductsAsync(Guid categoryId) =>
            _db.Products.AnyAsync(p => p.CategoryId == categoryId);

        public async Task<List<Category>> GetAllAsync()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task DeleteAsync(Guid categoryId)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(m => m.Id == categoryId);
            if (category == null) return;

            category.IsDeleted = true;
            await _db.SaveChangesAsync();
        }
    }
}
