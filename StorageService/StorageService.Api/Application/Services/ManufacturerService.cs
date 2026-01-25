using StorageService.Api.Application.DTOs;
using StorageService.Api.Application.Interfaces;
using StorageService.Api.Application.Mappers;
using StorageService.Api.Domain.Entities;
using StorageService.Api.Infrastructure.Interfaces;

namespace StorageService.Api.Application.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _repo;

        public ManufacturerService(IManufacturerRepository repo)
        {
            _repo = repo;
        }

        public async Task<ManufacturerDto> GetOrCreateAsync(string name)
        {
            var manufacturer = await _repo.GetByNameAsync(name);

            if (manufacturer != null)
                return manufacturer.ToDto();

            manufacturer = new Manufacturer
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            await _repo.AddAsync(manufacturer);
            return manufacturer.ToDto();
        }

        public async Task HandleUnusedAsync(Guid manufacturerId)
        {
            if (await _repo.HasProductsAsync(manufacturerId))
                return;

            await _repo.DeleteAsync(manufacturerId);
        }

        public async Task<ManufacturerDto?> GetAsync(Guid id)
        {
            var manufacturer = await _repo.GetByIdAsync(id);

            return manufacturer?.ToDto();
        }


        public async Task<List<ManufacturerDto>> GetAllAsync()
        {
            var manufacturers = await _repo.GetAllAsync();

            return manufacturers.Select(m => m.ToDto()).ToList();
        }
    }
}
