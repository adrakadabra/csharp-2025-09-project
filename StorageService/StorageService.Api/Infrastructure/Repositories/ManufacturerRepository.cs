using Microsoft.EntityFrameworkCore;
using StorageService.Api.Domain.Entities;
using StorageService.Api.Infrastructure.Data;
using StorageService.Api.Infrastructure.Interfaces;

namespace StorageService.Api.Infrastructure.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ApplicationDbContext _db;

        public ManufacturerRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Manufacturer?> GetByIdAsync(Guid id)
        {
            return await _db.Manufacturers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Manufacturer?> GetByNameAsync(string name)
        {
            return await _db.Manufacturers.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task AddAsync(Manufacturer manufacturer)
        {
            _db.Manufacturers.Add(manufacturer);
            await _db.SaveChangesAsync();
        }

        public Task<bool> HasProductsAsync(Guid manufacturerId) =>
            _db.Products.AnyAsync(p => p.ManufacturerId == manufacturerId);

        public async Task<List<Manufacturer>> GetAllAsync()
        {
            return await _db.Manufacturers.ToListAsync();
        }

        public async Task DeleteAsync(Guid manufacturerId)
        {
            var manufacturer = await _db.Manufacturers.FirstOrDefaultAsync(m => m.Id == manufacturerId);
            if (manufacturer == null) return;

            manufacturer.IsDeleted = true;
            await _db.SaveChangesAsync();
        }
    }
}
