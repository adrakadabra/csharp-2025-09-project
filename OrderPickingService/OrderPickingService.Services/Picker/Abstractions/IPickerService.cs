using OrderPickingService.Services.Picker.Dtos;

namespace OrderPickingService.Services.Picker.Abstractions;

public interface IPickerService
{
    Task<List<PickerDto>> GetAllPickersAsync();
    Task<PickerDto?> GetPickerByIdAsync(long id);
    Task<PickerDto> CreatePickerAsync(CreatePickerDto picker);
    Task<PickerDto> UpdatePikerAsync(long id, UpdatePikerDto updatePikerDto);
}