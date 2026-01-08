using OrderPickingService.Services.Picker.Abstractions;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Services.Picker;

internal sealed class PickerService(IPickerRepository pickerRepository) : IPickerService
{
    public Task<List<Domain.Entities.Picker>> GetPickers()
    {
        return pickerRepository.GetPickers();
    }
}