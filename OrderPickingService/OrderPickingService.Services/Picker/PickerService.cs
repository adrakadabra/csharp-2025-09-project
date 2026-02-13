using OrderPickingService.Services.Picker.Abstractions;
using OrderPickingService.Services.Picker.Dtos;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Services.Picker;

internal sealed class PickerService(IPickerRepository pickerRepository) : IPickerService
{
    public async Task<List<PickerDto>> GetAllPickersAsync(CancellationToken cancellationToken = default)
    {
        var pickers = await pickerRepository.GetAllAsync(cancellationToken);
        var pickerDtos = pickers
            .Select(x => x.ToPickerDto())
            .ToList();
        
        return pickerDtos;
    }
    
    public async Task<PickerDto?> GetPickerByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var picker = await pickerRepository.GetByIdAsync(id, cancellationToken);
        
        return picker?.ToPickerDto();
    }

    public async Task<PickerDto> CreatePickerAsync(CreatePickerDto pickerDto, CancellationToken cancellationToken = default)
    {
        var picker = await pickerRepository.CreateAsync(pickerDto.ToPicker(), cancellationToken);
        
        return picker.ToPickerDto();
    }

    public async Task<PickerDto> UpdatePikerAsync(UpdatePickerDto updatePickerDto, CancellationToken cancellationToken = default)
    {
        var currentPicker = await pickerRepository.GetByIdAsync(updatePickerDto.id, cancellationToken);
        
        if(currentPicker == null)
        {
            throw new KeyNotFoundException($"Picker with id = {updatePickerDto.id} not found");
        }
 
        if(!string.IsNullOrEmpty(updatePickerDto.FirstName))
            currentPicker.ChangeFirstName(updatePickerDto.FirstName);

        if(!string.IsNullOrEmpty(updatePickerDto.LastName))
            currentPicker.ChangeLastName(updatePickerDto.LastName);
        
        await pickerRepository.UpdateAsync(currentPicker, cancellationToken);
        
        return currentPicker.ToPickerDto();
    }
}