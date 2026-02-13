using OrderPickingService.Domain.Entities;

namespace OrderPickingService.Services.Repositories.Abstractions;

public interface IPickerRepository
{
    Task<List<Picker>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Picker?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Picker> CreateAsync(Picker picker, CancellationToken cancellationToken = default);
    Task UpdateAsync(Picker picker, CancellationToken cancellationToken = default);
}