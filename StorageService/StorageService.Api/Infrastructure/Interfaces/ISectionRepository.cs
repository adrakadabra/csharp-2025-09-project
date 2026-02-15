using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Infrastructure.Interfaces
{
    public interface ISectionRepository
    {
        Task<Section?> GetAsync(Guid id);
        Task<Section?> GetByCodeAsync(string code);
        Task<List<Section>> GetAllAsync();
        Task<Section> AddAsync(string code, string? description);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Section section);
    }
}
