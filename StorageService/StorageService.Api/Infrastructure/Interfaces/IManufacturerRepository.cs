using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Infrastructure.Interfaces
{
    public interface IManufacturerRepository
    {
        Task<Manufacturer?> GetByIdAsync(Guid id);
        Task<Manufacturer?> GetByNameAsync(string name);
        Task<List<Manufacturer>> GetAllAsync();
        Task AddAsync(Manufacturer manufacturer);
        Task<bool> HasProductsAsync(Guid manufacturerId);
        Task DeleteAsync(Guid manufacturerId);
    }
}
