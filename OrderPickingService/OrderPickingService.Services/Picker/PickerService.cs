using OrderPickingService.Services.Picker.Abstractions;
using OrderPickingService.Services.Picker.Dtos;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Services.Picker;

internal sealed class PickerService(IPickerRepository pickerRepository) : IPickerService
{
    public async Task<List<PickerDto>> GetAllPickersAsync()
    {
        var pickers = await pickerRepository.GetAllAsync();
        var pickerDtos = pickers
            .Select(x => PickerDto.Create(x.Id, x.FirstName, x.LastName))
            .ToList();
        
        return pickerDtos;
    }

    public async Task<PickerDto?> GetPickerByIdAsync(long id)
    {
        var picker = await pickerRepository.GetByIdAsync(id);
        
        return picker == null ? null : PickerDto.Create(picker.Id, picker.FirstName, picker.LastName);
    }

    public async Task<PickerDto> CreatePickerAsync(CreatePickerDto pickerDto)
    {
        if (string.IsNullOrEmpty(pickerDto.FirstName) ||
            string.IsNullOrEmpty(pickerDto.LastName))
        {
            throw new ArgumentException("First and last name are required");
        }
        
        var picker = await pickerRepository.CreateAsync(Domain.Entities.Picker.Create(pickerDto.FirstName, pickerDto.LastName));
        
        return PickerDto.Create(picker.Id, picker.FirstName, picker.LastName);
    }

    public async Task<PickerDto> UpdatePikerAsync(long id, UpdatePikerDto updatePikerDto)
    {
        var currentPicker = await pickerRepository.GetByIdAsync(id);
        
        if(currentPicker == null)
        {
            throw new KeyNotFoundException($"Picker with id = {id} not found");
        }

        if (string.IsNullOrEmpty(updatePikerDto.FirstName) &&
            string.IsNullOrEmpty(updatePikerDto.LastName))
        {
            throw new ArgumentException("First or last name are required");
        }
 
        if(!string.IsNullOrEmpty(updatePikerDto.FirstName))
            currentPicker.ChangeFirstName(updatePikerDto.FirstName);

        if(!string.IsNullOrEmpty(updatePikerDto.LastName))
            currentPicker.ChangeLastName(updatePikerDto.LastName);
        
        await pickerRepository.UpdateAsync(currentPicker);
        
        return PickerDto.Create(currentPicker.Id, currentPicker.FirstName, currentPicker.LastName);
    }
}