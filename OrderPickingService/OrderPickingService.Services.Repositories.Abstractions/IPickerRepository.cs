using OrderPickingService.Domain.Entities;

namespace OrderPickingService.Services.Repositories.Abstractions;

public interface IPickerRepository
{
    Task<List<Picker>> GetAllAsync();
    Task<Picker?> GetByIdAsync(long id);
    Task<Picker> CreateAsync(Picker picker);
    Task UpdateAsync(Picker picker);
}