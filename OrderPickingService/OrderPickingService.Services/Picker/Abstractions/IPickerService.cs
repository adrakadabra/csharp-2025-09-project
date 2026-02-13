using OrderPickingService.Services.Picker.Dtos;

namespace OrderPickingService.Services.Picker.Abstractions;

public interface IPickerService
{
    Task<List<PickerDto>> GetAllPickersAsync(CancellationToken cancellationToken = default);
    Task<PickerDto?> GetPickerByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PickerDto> CreatePickerAsync(CreatePickerDto picker, CancellationToken cancellationToken = default);
    Task<PickerDto> UpdatePikerAsync(UpdatePickerDto updatePickerDto, CancellationToken cancellationToken = default);
}