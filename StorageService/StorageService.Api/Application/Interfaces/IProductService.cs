using StorageService.Api.Application.DTOs;

namespace StorageService.Api.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<List<ProductDto>> GetBySectionIdAsync(Guid sectionId);
    Task<PagedResult<ProductDto>> GetPagedAsync(int page, int pageSize);
    Task<List<ProductLookupDto>> GetByIdsAsync(List<Guid> ids);
    Task<bool> UpdateAsync(Guid id, UpdateProductDto dto);
    Task<bool> DeleteAsync(Guid id);
}
