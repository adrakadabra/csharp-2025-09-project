using StorageService.Api.Application.DTOs;
using StorageService.Api.Application.Interfaces;
using StorageService.Api.Application.Mappers;
using StorageService.Api.Infrastructure.Interfaces;

namespace StorageService.Api.Application.Services
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _repo;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufService;

        public SectionService(ISectionRepository repo, IManufacturerService manufacturerService, ICategoryService categoryService, IProductRepository productRepo)
        {
            _repo = repo;
            _categoryService = categoryService;
            _manufService = manufacturerService;
            _productRepo = productRepo;
        }
        public async Task<SectionDto> CreateAsync(string code, string? description)
        {
            var newSection = await _repo.AddAsync(code, description);

            return newSection.ToDto();
        }

        public async Task DeleteAsync(Guid sectionId)
        {
            var section = await _repo.GetAsync(sectionId);
            if (section == null) throw new InvalidOperationException("No section for delete");

            var products = await _productRepo.GetBySectionIdAsync(sectionId);

            var affectedCategoryIds = products
                .Select(p => p.Category.Id)
                .Distinct()
                .ToList();

            var affectedManufacturerIds = products
                .Select(p => p.Manufacturer.Id)
                .Distinct()
                .ToList();

            await _repo.DeleteAsync(sectionId);

            foreach (var categoryId in affectedCategoryIds)
                await _categoryService.HandleUnusedAsync(categoryId);

            foreach (var manufacturerId in affectedManufacturerIds)
                await _manufService.HandleUnusedAsync(manufacturerId);
        }

        public async Task<List<SectionDto>> GetAllAsync()
        {
            var sections = await _repo.GetAllAsync();

            return sections.Select(s => s.ToDto()).ToList();
        }

        public async Task<SectionDto?> GetByCodeAsync(string code)
        {
            return (await _repo.GetByCodeAsync(code))?.ToDto();
        }

        public async Task<SectionDto?> GetByIdAsync(Guid sectionId)
        {
            return (await _repo.GetAsync(sectionId))?.ToDto();
        }

        public async Task UpdateAsync(Guid id, UpdateSectionDto section)
        {
            var existSection = await _repo.GetAsync(id);

            if (existSection == null) { throw new InvalidOperationException("No section for update"); }

            if (!string.IsNullOrEmpty(section.Code))
            {
                existSection.Code = section.Code;
            }

            if (!string.IsNullOrEmpty(section.Description))
            {
                existSection.Description = section.Description;
            }

            await _repo.UpdateAsync(existSection);
        }
    }
}
