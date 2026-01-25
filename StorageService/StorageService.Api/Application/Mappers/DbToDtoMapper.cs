using StorageService.Api.Application.DTOs;
using StorageService.Api.Domain.Entities;

namespace StorageService.Api.Application.Mappers
{
    public static class DbToDtoMapper
    {
        public static ProductDto ToDto(this Product product)
        {
            return new ProductDto
            {
                CreatedAt = product.CreatedAt,
                Description = product.Description,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                UpdatedAt = product.UpdatedAt,
                Category = product.Category.ToDto(),
                CreatedBy = product.CreatedBy,
                Manufacturer = product.Manufacturer.ToDto(),
                Section = product.Section.ToDto(),
                UpdatedBy = product.UpdatedBy,
            };
        }

        public static ManufacturerDto ToDto(this Manufacturer manufacturer)
        {
            return new ManufacturerDto
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                Country = manufacturer.Country,
            };
        }

        public static CategoryDto ToDto(this Category category)
        {
            return new CategoryDto
            {
                Name = category.Name,
                Id = category.Id,
                Description = category.Description
            };
        }

        public static SectionDto ToDto(this Section section)
        {
            return new SectionDto
            {
                Code = section.Code,
                Id = section.Id,
                Description = section.Description
            };
        }
    }
}
