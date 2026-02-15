namespace OrderPickingService.Infrastructure.Database.Entities.Picker;

internal static class PickerMappingExtensions
{
    public static Domain.Entities.Picker ToPicker(this PickerEntity pickerEntity)
    {
        return Domain.Entities.Picker.Load(pickerEntity.Id, pickerEntity.FirstName, pickerEntity.LastName);
    }
    
    public static PickerEntity ToPickerEntity(this Domain.Entities.Picker pickerEntity)
    {
        return PickerEntity.Create(pickerEntity.FirstName, pickerEntity.LastName);
    }
}