using StorageService.Api.Application.DTOs;

namespace StorageService.Api.Application.Interfaces
{
    public interface ISectionService
    {
        Task<SectionDto> CreateAsync(string code, string? description);
        Task<SectionDto?> GetByIdAsync(Guid sectionId);
        Task<SectionDto?> GetByCodeAsync(string code);
        Task<List<SectionDto>> GetAllAsync();
        Task UpdateAsync(Guid id, UpdateSectionDto section);
        Task DeleteAsync(Guid sectionId);
    }
}
