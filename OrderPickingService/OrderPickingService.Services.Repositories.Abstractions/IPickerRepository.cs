using OrderPickingService.Domain.Entities;

namespace OrderPickingService.Services.Repositories.Abstractions;

public interface IPickerRepository
{
    public Task<List<Picker>> GetPickers();
}