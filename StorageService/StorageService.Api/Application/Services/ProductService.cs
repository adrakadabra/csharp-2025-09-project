using StorageService.Api.Application.DTOs;
using StorageService.Api.Application.Interfaces;
using StorageService.Api.Infrastructure.Repositories;
using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var entity = new Product
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Quantity = dto.Quantity,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        var created = await _repo.AddAsync(entity);

        return new ProductDto(created.Id, created.Name, created.Description, created.Quantity, created.Price, created.CreatedAt, created.UpdatedAt);
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var p = await _repo.GetByIdAsync(id);
        if (p == null) return null;
        return new ProductDto(p.Id, p.Name, p.Description, p.Quantity, p.Price, p.CreatedAt, p.UpdatedAt);
    }

    public async Task<PagedResult<ProductDto>> GetPagedAsync(int page, int pageSize)
    {
        var (items, total) = await _repo.GetPagedAsync(page, pageSize);
        var dtoItems = items.Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Quantity, p.Price, p.CreatedAt, p.UpdatedAt)).ToArray();
        return new PagedResult<ProductDto> { Items = dtoItems, Page = page, PageSize = pageSize, Total = total };
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        var p = await _repo.GetByIdAsync(id);
        if (p == null) return false;
        p.Update(dto.Name, dto.Description, dto.Quantity, dto.Price);
        await _repo.UpdateAsync(p);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var p = await _repo.GetByIdAsync(id);
        if (p == null) return false;
        await _repo.DeleteAsync(p);
        return true;
    }
}
