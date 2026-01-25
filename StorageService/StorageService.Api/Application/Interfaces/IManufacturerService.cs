using StorageService.Api.Application.DTOs;

namespace StorageService.Api.Application.Interfaces
{
    public interface IManufacturerService
    {
        Task<ManufacturerDto> GetOrCreateAsync(string name);
        Task HandleUnusedAsync(Guid categoryId);
        Task<ManufacturerDto?> GetAsync(Guid id);
        Task<List<ManufacturerDto>> GetAllAsync();
    }
}
