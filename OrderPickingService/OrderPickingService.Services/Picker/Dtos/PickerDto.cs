namespace OrderPickingService.Services.Picker.Dtos;

public sealed record PickerDto(
    long Id,
    string FirstName,
    string LastName)
{
    public string FullName => $"{FirstName} {LastName}";
    
     public static PickerDto Create(long id, string firstName, string lastName)
     {
         return new PickerDto(id, firstName, lastName);
     }
}
