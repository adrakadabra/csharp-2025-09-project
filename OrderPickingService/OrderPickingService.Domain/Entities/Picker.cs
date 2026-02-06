namespace OrderPickingService.Domain.Entities;

public sealed class Picker
{
    public long Id { get; }
    public string FirstName { get; private set;  }
    public string LastName { get; private set;  }

    public static Picker Create(string firstName, string lastName)
    {
        return new Picker(0, firstName, lastName);
    }
    
    public static Picker Load(long id, string firstName, string lastName)
    {
        return new Picker(id, firstName, lastName);
    }

    public void ChangeFirstName(string newFirstName)
    {
        if (string.IsNullOrWhiteSpace(newFirstName))
        {
            throw new ArgumentException("First name required");
        }
        
        FirstName = newFirstName;
    }
    public void ChangeLastName(string newLastName)
    {
        if (string.IsNullOrWhiteSpace(newLastName))
        {
            throw new ArgumentException("Last name required");
        }
        
        LastName = newLastName;
    }
    
    private Picker(long id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }
}