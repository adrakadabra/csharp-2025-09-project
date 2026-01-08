namespace OrderPickingService.Services.Picker.Abstractions;

public interface IPickerService
{
    public Task<List<Domain.Entities.Picker>> GetPickers();
}