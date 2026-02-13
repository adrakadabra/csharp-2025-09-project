using OrderPickingService.Services.Picker.Dtos;

namespace OrderPickingService.Services.Picker;

internal static class PickerMappingExtensions
{
    public static PickerDto ToPickerDto(this Domain.Entities.Picker picker)
    {
        return PickerDto.Create(picker.Id, picker.FirstName, picker.LastName);
    }

    public static Domain.Entities.Picker ToPicker(this CreatePickerDto dto)
    {
        return Domain.Entities.Picker.Create(dto.FirstName, dto.LastName);
    }
}