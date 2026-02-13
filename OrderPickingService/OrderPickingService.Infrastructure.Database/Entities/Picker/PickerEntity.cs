namespace OrderPickingService.Infrastructure.Database.Entities.Picker;

internal class PickerEntity : BaseEntity
{
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public PickerEntity() { }

    public static PickerEntity Create(string firstName, string lastName)
    {
        return new PickerEntity
        {
            FirstName = firstName,
            LastName = lastName
        };
    }
}