using StorageService.Api.Application.DTOs;
using StorageService.Api.Application.Interfaces;
using StorageService.Api.Application.Mappers;
using StorageService.Api.Domain.Entities;
using StorageService.Api.Infrastructure.Interfaces;

namespace StorageService.Api.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ISectionService _sectionService;

        public ProductService(IProductRepository repo, ICategoryService categoryService,
            IManufacturerService manufacturerService, ISectionService sectionService)
        {
            _repo = repo;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _sectionService = sectionService;
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var section = await _sectionService.GetByCodeAsync(dto.SectionCode);

            if (section == null) { throw new InvalidOperationException("Not exist section code"); }

            var category = await _categoryService.GetOrCreateAsync(dto.CategoryName);
            var manufacturer = await _manufacturerService.GetOrCreateAsync(dto.ManufacturerName);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity,
                CategoryId = category.Id,
                ManufacturerId = manufacturer.Id,
                SectionId = section.Id,
                CreatedAt = DateTime.UtcNow,
                //todo: from claims
                CreatedBy = "user"
            };

            var newProduct = await _repo.AddAsync(product);

            return newProduct.ToDto();
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            var product = await _repo.GetByIdAsync(id);
            return product?.ToDto();
        }

        public async Task<PagedResult<ProductDto>> GetPagedAsync(int page, int pageSize)
        {
            var (items, total) = await _repo.GetPagedAsync(page, pageSize);
            var dtoItems = items.Select(p => p.ToDto()).ToArray();
            return new PagedResult<ProductDto> { Items = dtoItems, Page = page, PageSize = pageSize, Total = total };
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateProductDto dto)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) throw new Exception();

            if (!string.IsNullOrEmpty(dto.SectionCode))
            {
                var section = await _sectionService.GetByCodeAsync(dto.SectionCode);

                if (section == null) { throw new InvalidOperationException("Not exist section code"); }

                product.SectionId = section.Id;
            }

            if (!string.IsNullOrEmpty(dto.CategoryName))
            {
                var oldCategoryId = product.CategoryId;
                var newCategory = await _categoryService.GetOrCreateAsync(dto.CategoryName);
                await _categoryService.HandleUnusedAsync(oldCategoryId);
                product.CategoryId = newCategory.Id;
            }

            if (!string.IsNullOrEmpty(dto.ManufacturerName))
            {
                var oldManufacturerId = product.ManufacturerId;
                var newManufacturer = await _categoryService.GetOrCreateAsync(dto.ManufacturerName);
                await _manufacturerService.HandleUnusedAsync(oldManufacturerId);
                product.ManufacturerId = newManufacturer.Id;
            }

            if (!string.IsNullOrEmpty(dto.Description)) { product.Description = dto.Description; }

            if (!string.IsNullOrEmpty(dto.Name)) { product.Name = dto.Name; }

            if (dto.Price.HasValue)
            {
                product.Price = dto.Price.Value;
            }

            if (dto.Quantity.HasValue)
            {
                product.Quantity = dto.Quantity.Value;
            }

            product.UpdatedAt = DateTime.UtcNow;
            //todo: from claims
            product.UpdatedBy = "user";

            await _repo.UpdateAsync(product);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return false;
            var categoryId = product.CategoryId;
            var manuId = product.ManufacturerId;
            await _categoryService.HandleUnusedAsync(categoryId);
            await _manufacturerService.HandleUnusedAsync(manuId);

            await _repo.DeleteAsync(product);
            return true;
        }

        public async Task<List<ProductDto>> GetBySectionIdAsync(Guid sectionId)
        {
            var res = await _repo.GetBySectionIdAsync(sectionId);

            return res.Select(s => s.ToDto()).ToList();
        }
    }
}