namespace OrderPickingService.Domain.Entities;

public sealed class Picker
{
    public long Id { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public static Picker Create(long id, string firstName, string lastName)
    {
        return new Picker(id, firstName, lastName);
    }
    
    private Picker(long id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }
}