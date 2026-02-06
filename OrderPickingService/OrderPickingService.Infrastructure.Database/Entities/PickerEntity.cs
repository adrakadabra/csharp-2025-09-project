namespace OrderPickingService.Infrastructure.Database.Entities;

internal class PickerEntity : BaseEntity
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public static PickerEntity Create(string firstName, string lastName)
    {
        return new PickerEntity(0, firstName, lastName);
    }
    
    public static PickerEntity Create(long id, string firstName, string lastName)
    {
        return new PickerEntity(id, firstName, lastName);
    }

    public PickerEntity()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
    }
    
    private PickerEntity(
        long id, 
        string firstName, 
        string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }
}